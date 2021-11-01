unit officedriver_contratos_fechamentos;

interface

uses
  wtsServerObjs, SysUtils, DateUtils;

type
  TTipo = (tRepasse, tCobranca);
  TTipoContrato = (ttFixo, ttVariavel);
  TBaseCaculo = (bcMensal, bcDiario, bcHora);
  TTipoPeriodo = (tpNormal = 0, tpFds = 1, tpFeriado = 2);

Type
  TValorPeriodo = record
    Descricao: String;
    Normal,
    Excedente: Double;
    Tipo: Integer;
  end;

implementation

function HoraToDecimal(AHorario: string): Double;
begin
  Result := StrToInt(Copy(AHorario, 1,2)) + (StrToInt(Copy(AHorario, 4,2)) / 60);
end;


procedure CalcularPeriodo(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
  C: IwtsCommand;
  Periodos, Detalhe, Feriados: IwtsWriteData;
  Entrada, Saida: TDateTime;
  ValorPeriodo: TValorPeriodo;

  function ObterValorPeriodo(const AData: IwtsWriteData; const AHora: string;
     AEntrada: TDateTime): TValorPeriodo;
  var Tipo: TTipoPeriodo;
  begin
     Tipo := tpNormal;

    if Feriados.Locate(['Data'],[AEntrada]) then
     Tipo := tpFeriado
    else if (DayOfWeek(AEntrada) = 1) or (DayOfWeek(AEntrada) = 7) then
      Tipo := tpFds;

    AData.First;
    Result.Normal    := 0;
    Result.Excedente := 0;
    Result.Tipo      := 0;
    Result.Descricao := '';

    while not AData.Eof do
    begin
      if (AHora >= AData.Value['Inicio']) and (AHora <= AData.Value['Fim']) and
         ((AData.Value['Tipo'] = Tipo) or (AData.Value['Tipo'] = tpNormal)) then
      begin
       Result.Descricao := AData.Value['Descricao'];
       Result.Normal    := AData.Value['ValorNormal'];
       Result.Excedente := AData.Value['ValorExcedente'];
       Result.Tipo      := AData.Value['Tipo'];
       Exit;
      end;
      AData.Next;
    end;
  end;
begin
  C := DataPool.Open('MILLENIUM');
  Periodos := Input.AsData['Periodos'];
  Entrada := Input.Value['Entrada'];   
  Saida := Input.Value['Saida'];   

  C.Dim('Inicio', YearOf(Entrada));
  C.Dim('Inicio', YearOf(Saida));
  C.Execute('SELECT Data FROM Feriados where #YEAR(Data) between (:AnoInicio and :AnoFim)');
  Feriados := C.CreateRecordset;

  C.Dim('Entrada',Entrada);
  C.Dim('Saida',Saida);
  C.Execute('#CALL OFFICEDRIVER.APONTAMENTOS.Detalhar(:Entrada,:Saida);');
  Detalhe := C.CreateRecordset;
  while not Detalhe.EOF do
  begin
     ValorPeriodo := ObterValorPeriodo(Periodos,Detalhe.Value['Horario'], Entrada);

      if (not Cooperados.Locate(['Cooperado'],[Apontamentos.Value['Cooperado']])) then
       Raise Exception.Create('Cooperado '+GetNomeCooperado(Apontamentos.Value['Cooperado'])+' não encontrado no contrato.');

     if (TotalHoraCalculada > 0) and
       (((Contrato.GetFieldByName('BaseCalculo') = 'D') and
         (Apontamentos.value['DataEntrada'] <> DataEntrada)) or (Apontamentos.value['Cooperado'] <> Cooperado))  then
     begin
       ValorCobranca := ObterValorHoraDesc(CondicoesCobranca, Cooperados.Value['Funcao'], Apontamentos.value['DataEntrada']);
       ValorRepasse  := ObterValorHoraDesc(CondicoesRepasse , Cooperados.Value['Funcao'], Apontamentos.value['DataEntrada']);

       Output.NewRecord;
       Output.SetFieldByName('Apontamento',Apontamentos.Value['Apontamento']);
       Output.SetFieldByName('ApontamentoDetalhado',Apontamentos.Value['ApontamentoDetalhado']);
       Output.SetFieldByName('Descricao','Desconto de Intervalo');
       Output.SetFieldByName('ValorRepasse',  (ValorRepasse.Normal  * Intervalo)*-1);
       Output.SetFieldByName('ValorCobranca', (ValorCobranca.Normal * Intervalo)*-1);
       Output.SetFieldByName('PeriodoCobranca',ValorCobranca.Periodo);
       Output.SetFieldByName('PeriodoRepasse' ,ValorRepasse.Periodo);
       TotalHoraCalculada := 0;
     end;

     TotalHoraCalculada := TotalHoraCalculada + Apontamentos.Value['Tempo'];
     ValorCobranca := ObterValorHora(CondicoesCobranca, Apontamentos.Value['Horario'], Cooperados.Value['Funcao'], Apontamentos.value['DataEntrada']);
     ValorRepasse  := ObterValorHora(CondicoesRepasse , Apontamentos.Value['Horario'], Cooperados.Value['Funcao'], Apontamentos.value['DataEntrada']);
     Output.NewRecord;
     Output.SetFieldByName('Apontamento',Apontamentos.Value['Apontamento']);
     Output.SetFieldByName('ApontamentoDetalhado',Apontamentos.Value['ApontamentoDetalhado']);
     Output.SetFieldByName('Horario',Apontamentos.Value['Horario']);
     if TotalHoraCalculada <= ValorRepasse.LimiteHorasTrabalhadas then
       Output.SetFieldByName('ValorRepasse', ValorRepasse.Normal * Apontamentos.Value['Tempo'])
    else
      Output.SetFieldByName('ValorRepasse', ValorRepasse.Excedente * Apontamentos.Value['Tempo']);

     if TotalHoraCalculada <= ValorCobranca.LimiteHorasTrabalhadas then
       Output.SetFieldByName('ValorCobranca', ValorCobranca.Normal * Apontamentos.Value['Tempo'])
     else
       Output.SetFieldByName('ValorCobranca', ValorCobranca.Excedente * Apontamentos.Value['Tempo']);

     Output.SetFieldByName('Descricao','Apontamento');
     Output.SetFieldByName('PeriodoCobranca',ValorCobranca.Periodo);
     Output.SetFieldByName('PeriodoRepasse' ,ValorRepasse.Periodo);

     Cooperado := Apontamentos.value['Cooperado'];
     DataEntrada := Apontamentos.value['DataEntrada'];

    Detalhe.Next;
  end;
end;

procedure FecharContrato(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   C: IwtsCommand;
   ApontamentosCalculados: IwtsData;
   Evento, TipoContrato, ValorContrato: Integer;
   DataInicial, DataFinal: TDateTime;
   BaseCalculo: TBaseCaculo;
   Contrato, CondicoesComerciaisCooperado, CondicaoComercial,Apontamentos,DetalheApontamento,
   CooperadosContratos, Cooperados, Extrato: IwtsWriteData;
   i: Integer;
   Data: TDateTime;
begin
//  C := DataPool.Open('MILLENIUM');
//
//  C.Execute('SELECT DataBaseFechamento FROM Fechamentos where Fechamento = :Fechamento');
//  DataFinal := C.Value['DataBaseFechamento'];
//  DataInicial := IncMonth(DataFinal, -1);
//  C.Execute('#CALL OFFICEDRIVER.CONTRATOS.Consultar(:Contrato)');
//  Contrato := C.CreateRecordset;
//
//  CondicoesComerciaisCooperado := Contrato.AsData['CondComCooperado'] as IwtsWriteData;
//  CondicaoComercial := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.CONDICAOCOMERCIALCOOPERADO');
//  Extrato := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULO.EXTRATO');
//
//  while not CondicoesComerciaisCooperado.EOF do
//  begin
//    BaseCalculo := CondicoesComerciaisCooperado.Value['BASECALCULO'];
//
//    CondicaoComercial.Clear;
//    CondicaoComercial.New;
//    for i := 0 to CondicoesComerciaisCooperado.FieldCount - 1 do
//      CondicaoComercial.AtIndex[i] := CondicoesComerciaisCooperado.AtIndex[i];
//    CondicaoComercial.Add;
//
//    C.Dim('Fechamento',Input.Value['Fechamento']);
//    C.Dim('Contrato',Input.Value['Contrato']);
//    C.Dim('Funcao',CondicaoComercial.Value['Funcao']);
//    C.Execute('#CALL OFFICEDRIVER.CONTRATOS.FECHAMENTOS.ListarApontamentos(:Fechamento,:Contrato,:Funcao);');
//    Apontamentos := C.AsData['Apontamentos'] as IwtsWriteData; 
//
//    CooperadosContratos := Contrato.AsData['Cooperados'] as IwtsWriteData;
//    CooperadosContratos.Filter := 'FUNCAO='+CondicoesComerciaisCooperado.Value['Funcao'];
//    while not CooperadosContratos.EOF do
//    begin
//      for Data := DataInicial to DataFinal do
//      begin
//        if Apontamentos.Locate(['Cooperado','DataEntrada'],[CooperadosContratos.Value['Cooperado'],Data]) then
//        begin
//          
//          Extrato.New;
//          Extrato.Value['DataEntrada'] := Apontamentos.Value['DataEntrada'];
//          Extrato.Value['DiaSemanaEntrada'] := FormatDateTime('dddd',Apontamentos.Value['Entrada']);
//          Extrato.Value['HoraEntrada'] := FormatDateTime('hh:nn',Apontamentos.Value['Entrada']);
//          Extrato.Value['DataSaida'] := FormatDateTime('dd/mm/yyyy',Apontamentos.Value['Saida']);
//          Extrato.Value['DiaSemanaSaida'] := FormatDateTime('dddd',Apontamentos.Value['Saida']);
//          Extrato.Value['HoraSaida'] := FormatDateTime('hh:nn',Apontamentos.Value['Saida']);
//          Extrato.Value['Trabalhada'] := FormatDateTime('hh:nn',Apontamentos.Value['Saida'] - Apontamentos.Value['Entrada']);
//          Extrato.Value['Intervalo'] := Apontamentos.Value['Intervalo'];
//          Extrato.Value['Total'] := FormatDateTime('hh:nn',Apontamentos.Value['Saida'] - Apontamentos.Value['Entrada'] - HoraToDecimal(Apontamentos.Value['Intervalo']));
//          Extrato.Add;
//        end else
//        begin
//          Extrato.New;
//          Extrato.Value['DataEntrada'] := Data;
//          Extrato.Value['DiaSemanaEntrada'] := FormatDateTime('dddd',Data);
//          Extrato.Value['HoraEntrada'] := '00:00';
//          Extrato.Value['DataSaida'] := FormatDateTime('dd/mm/yyyy',Data);
//          Extrato.Value['DiaSemanaSaida'] := FormatDateTime('dddd',Data);
//          Extrato.Value['HoraSaida'] := '00:00';
//          Extrato.Value['Trabalhada'] := '00:00';
//          Extrato.Value['Intervalo'] := '00:00';
//          Extrato.Value['Total'] := '00:00';
//          Extrato.Add;
//        end;
//      end;
//      Cooperados.New;
//      Cooperados.Value['Cooperado'] := CooperadosContratos.Value['Cooperado'];
//      Cooperados.Value['Exrato'] := Extrato.Data;
//      Cooperados.Add;
//
//      CooperadosContratos.Next;
//    end;
//
//
//    case BaseCalculo of
//      bcMensal:
//      begin
//        C.DimAsData('Contrato',Contrato);
//        C.DimAsData('CondicaoComercialCooperado',CondicaoComercial);
//        C.DimAsData('Cooperados',Cooperados);
//        C.Execute('#CALL OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CalcularContratoMes(:Contrato,:CondicaoComercialCooperado,:Cooperados)');
//      end;
//
//      bcDiario:
//      begin
//
//      end;
//
//      bcHora:
//      begin
//      end;
//    end;
//
//    //Insert
//
//
//    CondicoesComerciaisCooperado.Next;
//  end;
//
//
//  C.Execute('#CALL OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULARCONTRATO(:Fechamento,:Contrato)');
//  ApontamentosCalculados := C.CreateRecordset;
//
//  if ApontamentosCalculados.EOF then
//    raise Exception.Create('Não existem apontamentos em aberto para data selecionada.');
//
//  TipoContrato  := -1;
//  ValorContrato := 0;
//  C.Execute('SELECT Tipo, Valor FROM Contratos WHERE Id = :Contrato');
//  if not C.Eof then
//  begin
//    TipoContrato := C.GetFieldByName('Tipo');
//    ValorContrato := C.GetFieldByName('Valor');
//  end;
//
//  while not ApontamentosCalculados.EOF do
//  begin
//     C.Dim('Fechamento',Input.Value['Fechamento']);
//     C.Dim('ValorRepasse', ApontamentosCalculados.Value['ValorRepasse']);
//     C.Dim('ValorCobranca', ApontamentosCalculados.Value['ValorCobranca']);
//     C.Dim('Horario', ApontamentosCalculados.Value['Horario']);
//     C.Dim('PeriodoRepasse', ApontamentosCalculados.Value['PeriodoRepasse']);
//     C.Dim('PeriodoCobranca', ApontamentosCalculados.Value['PeriodoCobranca']);
//     C.Dim('Descricao', ApontamentosCalculados.Value['Descricao']);
//     C.Execute('INSERT INTO FechamentosEventos(Fechamento,ValorRepasse,ValorCobranca,Horario,PeriodoRepasse,PeriodoCobranca,Descricao) '+
//     '                                 VALUES(:Fechamento,:ValorRepasse,:ValorCobranca,:Horario,:PeriodoRepasse,:PeriodoCobranca,:Descricao) #RETURN(ID)');
//     Evento := C.value['Id'];
//     C.Dim('ApontamentoDetalhado', ApontamentosCalculados.Value['ApontamentoDetalhado']);
//     C.Dim('Evento', Evento);
//     C.Execute('Update ApontamentosDetalhados set Evento = :Evento WHERE Id = :ApontamentoDetalhado;');
//
//     C.Dim('Fechamento',Input.Value['Fechamento']);
//     C.Dim('Apontamento', ApontamentosCalculados.Value['Apontamento']);
//     C.Execute('Update Apontamentos set Fechamento = :Fechamento WHERE Id = :Apontamento;');
//
//     ApontamentosCalculados.Next;
//  end;
//
//  C.Execute('Update FechamentosContratos set ValorCobranca = (SELECT SUM(ValorCobranca) FROM FechamentosEventos WHERE Fechamento = :Fechamento), '+
//            '                                ValorRepasse  = (SELECT SUM(ValorRepasse)  FROM FechamentosEventos WHERE Fechamento = :Fechamento) '+
//            'WHERE Fechamento = :Fechamento and Contrato = :Contrato;');
//  if TipoContrato = 0 then
//  begin
//     C.Dim('ValorContrato',ValorContrato);
//     C.Execute('Update FechamentosContratos set ValorCobranca = :ValorContrato '+
//              'WHERE Fechamento = :Fechamento and Contrato = :Contrato;');
//
//  end;
end;

//
//procedure CalcularContrato(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
//var
//   C: IwtsCommand;
//   Fechamento,Contrato,Apontamentos,ApontamentosDetalhado,CondicoesCobranca,
//   CondicoesRepasse,Cooperados,Feriados: IwtsWriteData;
//   ValorCobranca, ValorRepasse: TValores;
//   TotalHoraCalculada, Intervalo, ValorContrato: Double;
//   DataEntrada: TDateTime;
//   Cooperado: Integer;
//
//   function GetNomeCooperado(Cooperado: Integer): String;
//   begin
//     C.Dim('Cooperado', Cooperado);
//     C.Execute('Select Nome From Cooperados where Id = :Cooperado');
//     Result := C.GetFieldByName('Nome');
//   end; 
//
//   procedure PreparaCondicao;
//   var FimOriginal, RecNo: string;
//       Tipo, Periodo, Funcao, Horas: Integer;
//       ValorNormal, ValorExcedente: Double;
//   begin
//     CondicoesRepasse.First;
//     while not CondicoesRepasse.EOF do
//     begin
//       if CondicoesRepasse.Value['Inicio'] > CondicoesRepasse.Value['Fim'] then
//       begin
//         FimOriginal    := CondicoesRepasse.Value['Fim'];
//         Tipo           := CondicoesRepasse.Value['Tipo'];
//         ValorNormal    := CondicoesRepasse.Value['ValorNormal'];
//         ValorExcedente := CondicoesRepasse.Value['ValorExcedente'];
//         Horas          := CondicoesCobranca.Value['Horas'];
//         Periodo        := CondicoesRepasse.Value['Periodo'];
//         Funcao         := CondicoesRepasse.Value['Funcao'];
//         RecNo          := CondicoesRepasse.GetBookmark;
//         CondicoesRepasse.Value['Fim'] := '23:59';
//         CondicoesRepasse.Update;
//         CondicoesRepasse.New;
//         CondicoesRepasse.Value['Inicio']         := '00:00';
//         CondicoesRepasse.Value['Fim']            := FimOriginal;
//         CondicoesRepasse.Value['Tipo']           := Tipo;
//         CondicoesRepasse.Value['ValorNormal']    := ValorNormal;
//         CondicoesRepasse.Value['ValorExcedente'] := ValorExcedente;
//         CondicoesRepasse.Value['Periodo']        := Periodo;
//         CondicoesRepasse.Value['Horas']          := Horas;
//         CondicoesRepasse.Value['Funcao']         := Funcao;
//         CondicoesRepasse.Add;
//         CondicoesRepasse.SetBookmark(RecNo);
//       end;
//       CondicoesRepasse.Next;
//     end;
//     CondicoesRepasse.First;
//
//     CondicoesCobranca.First;
//     while not CondicoesCobranca.EOF do
//     begin
//       if CondicoesCobranca.Value['Inicio'] > CondicoesCobranca.Value['Fim'] then
//       begin
//         FimOriginal    := CondicoesCobranca.Value['Fim'];
//         Tipo           := CondicoesCobranca.Value['Tipo'];
//         ValorNormal    := CondicoesCobranca.Value['ValorNormal'];
//         ValorExcedente := CondicoesCobranca.Value['ValorExcedente'];
//         Horas          := CondicoesCobranca.Value['Horas'];
//         Periodo        := CondicoesCobranca.Value['Periodo'];
//         Funcao         := CondicoesCobranca.Value['Funcao'];
//         RecNo          := CondicoesCobranca.GetBookmark;
//
//         CondicoesCobranca.Value['Fim'] := '23:59';
//         CondicoesCobranca.Update;
//         CondicoesCobranca.New;
//         CondicoesCobranca.Value['Inicio']         := '00:00';
//         CondicoesCobranca.Value['Fim']            := FimOriginal;
//         CondicoesCobranca.Value['Tipo']           := Tipo;
//         CondicoesCobranca.Value['ValorNormal']    := ValorNormal;
//         CondicoesCobranca.Value['ValorExcedente'] := ValorExcedente;
//         CondicoesCobranca.Value['Periodo']        := Periodo;
//         CondicoesCobranca.Value['Horas']          := Horas;
//         CondicoesCobranca.Value['Funcao']         := Funcao;
//         CondicoesCobranca.Add;
//         CondicoesCobranca.SetBookmark(RecNo);
//       end;
//       CondicoesCobranca.Next;
//     end;
//     CondicoesCobranca.First;
//   end;
//
//   function ObterValorHoraDesc(const AData: IwtsWriteData;
//     AFuncao: Integer; AEntrada: TDateTime): TValores;
//   var Tipo: Integer;
//   begin
//     Tipo := 1;
//
//     if Feriados.Locate(['Data'],[AEntrada]) then
//       Tipo := 2
//     else if (DayOfWeek(AEntrada) = 1) or (DayOfWeek(AEntrada) = 7) then
//       Tipo := 3;
//
//     AData.First;
//     Result.Normal                 := 0;
//     Result.Excedente              := 0;
//     Result.LimiteHorasTrabalhadas := 0;
//     Result.Periodo                := 0;
//     while not AData.Eof do
//     begin
//       if (AData.Value['Funcao'] = AFuncao) and
//         ((AData.Value['Tipo'] = Tipo) or (AData.Value['Tipo'] = 1)) then
//       begin
//         Result.Normal                 := AData.Value['ValorNormal'];
//         Result.Excedente              := AData.Value['ValorExcedente'];
//         Result.LimiteHorasTrabalhadas := AData.Value['Horas'];
//         Result.Periodo                := AData.Value['Periodo'];
//         Exit;
//       end;
//       AData.Next;
//     end;
//   end;
//
//   function ObterValorHora(const AData: IwtsWriteData; const AHora: string;
//     AFuncao: Integer; AEntrada: TDateTime): TValores;
//   var Tipo: Integer;
//   begin
//     Tipo := 1;
//
//     if Feriados.Locate(['Data'],[AEntrada]) then
//       Tipo := 2
//     else if (DayOfWeek(AEntrada) = 1) or (DayOfWeek(AEntrada) = 7) then
//       Tipo := 3;
//
//     AData.First;
//     Result.Normal                 := 0;
//     Result.Excedente              := 0;
//     Result.LimiteHorasTrabalhadas := 0;
//     Result.Periodo                := 0;
//     while not AData.Eof do
//     begin
//       if (AData.Value['Funcao'] = AFuncao) and
//          (AHora >= AData.Value['Inicio']) and (AHora <= AData.Value['Fim']) and
//          ((AData.Value['Tipo'] = Tipo) or (AData.Value['Tipo'] = 1)) then
//       begin
//         Result.Normal                 := AData.Value['ValorNormal'];
//         Result.Excedente              := AData.Value['ValorExcedente'];
//         Result.LimiteHorasTrabalhadas := AData.Value['Horas'];
//         Result.Periodo                := AData.Value['Periodo'];
//         Exit;
//       end;
//       AData.Next;
//     end;
//   end;
//begin
//  C := DataPool.Open('MILLENIUM');
//
//  C.Dim('Fechamento', Input.Value['Fechamento']);
//  C.Execute('SELECT * FROM Fechamentos WHERE Id =:Fechamento');
//  Fechamento := C.CreateRecordset;
//
//  C.Dim('Contrato', Input.Value['Contrato']);
//  C.Execute('SELECT * FROM Contratos WHERE Id =:Contrato');
//  Contrato := C.CreateRecordset;
//
//  C.Dim('Contrato', Input.Value['Contrato']);
//  C.Execute('SELECT * FROM ContratosCooperados cc WHERE cc.Contrato =:Contrato');
//  Cooperados := C.CreateRecordset;
//
//  C.Dim('Contrato', Input.Value['Contrato']);
//  C.Execute('SELECT ccc.Funcao, ccc.Horas, ccp.Id as Periodo, ccp.Descricao, ccp.Inicio, ccp.Fim, '+
//            'ccp.ValorNormal, ccp.ValorExcedente, ccp.Tipo '+
//            ' FROM ContratosCondComercialCoop ccc '+ 
//            ' INNER JOIN CondicaoComPeriodo ccp ' +
//            ' ON ccp.condicao = ccc.id '+
//            ' WHERE ccc.Contrato =:Contrato and ccc.Tipo = "C"');
//  CondicoesCobranca := C.CreateRecordset;
//
//  C.Dim('Contrato', Input.Value['Contrato']);
//  C.Execute('SELECT ccc.Funcao, ccc.Horas, ccp.Id as Periodo, ccp.Descricao, ccp.Inicio, ccp.Fim, '+
//            'ccp.ValorNormal, ccp.ValorExcedente, ccp.Tipo '+
//            ' FROM ContratosCondComercialCoop ccc '+ 
//            ' INNER JOIN CondicaoComPeriodo ccp ' +
//            ' ON ccp.condicao = ccc.id '+
//            ' WHERE ccc.Contrato =:Contrato and ccc.Tipo = "R"');
//  CondicoesRepasse := C.CreateRecordset;
//
//  C.Dim('Contrato', Input.Value['Contrato']);
//  C.Dim('DataBaseFechamento', Fechamento.Value['DataBaseFechamento']);
//  C.Execute('SELECT Ap.Id as Apontamento, Ad.Id as ApontamentoDetalhado, Ap.DataEntrada, Ad.Horario, Ad.Tempo, Ap.Cooperado FROM ApontamentosDetalhados Ad Inner Join Apontamentos Ap   '+
//            'ON Ad.Apontamento = Ap.Id '+
//            ' WHERE Ap.Contrato =:Contrato AND Ap.Fechamento IS NULL and Ap.DataEntrada <= :DataBaseFechamento '+
//            ' Order by Ap.Cooperado');
//  Apontamentos := C.CreateRecordset;
//
//  C.Dim('DataBaseFechamento', Fechamento.Value['DataBaseFechamento']);
//  C.Execute('SELECT Data FROM Feriados where #YEAR(Data) = #YEAR(:DataBaseFechamento)');
//  Feriados := C.CreateRecordset;
//
//  TotalHoraCalculada := 0;
//
//  PreparaCondicao;
//
//  Intervalo := HoraToDecimal(Contrato.Value['IntervaloDiaCooperado']);
//  while not Apontamentos.EOF do
//  begin
//     if (not Cooperados.Locate(['Cooperado'],[Apontamentos.Value['Cooperado']])) then
//       Raise Exception.Create('Cooperado '+GetNomeCooperado(Apontamentos.Value['Cooperado'])+' não encontrado no contrato.');
//
//     if (TotalHoraCalculada > 0) and
//       (((Contrato.GetFieldByName('BaseCalculo') = 'D') and
//         (Apontamentos.value['DataEntrada'] <> DataEntrada)) or (Apontamentos.value['Cooperado'] <> Cooperado))  then
//     begin
//       ValorCobranca := ObterValorHoraDesc(CondicoesCobranca, Cooperados.Value['Funcao'], Apontamentos.value['DataEntrada']);
//       ValorRepasse  := ObterValorHoraDesc(CondicoesRepasse , Cooperados.Value['Funcao'], Apontamentos.value['DataEntrada']);
//
//       Output.NewRecord;
//       Output.SetFieldByName('Apontamento',Apontamentos.Value['Apontamento']);
//       Output.SetFieldByName('ApontamentoDetalhado',Apontamentos.Value['ApontamentoDetalhado']);
//       Output.SetFieldByName('Descricao','Desconto de Intervalo');
//       Output.SetFieldByName('ValorRepasse',  (ValorRepasse.Normal  * Intervalo)*-1);
//       Output.SetFieldByName('ValorCobranca', (ValorCobranca.Normal * Intervalo)*-1);
//       Output.SetFieldByName('PeriodoCobranca',ValorCobranca.Periodo);
//       Output.SetFieldByName('PeriodoRepasse' ,ValorRepasse.Periodo);
//       TotalHoraCalculada := 0;
//     end;
//     
//     TotalHoraCalculada := TotalHoraCalculada + Apontamentos.Value['Tempo'];
//     ValorCobranca := ObterValorHora(CondicoesCobranca, Apontamentos.Value['Horario'], Cooperados.Value['Funcao'], Apontamentos.value['DataEntrada']);
//     ValorRepasse  := ObterValorHora(CondicoesRepasse , Apontamentos.Value['Horario'], Cooperados.Value['Funcao'], Apontamentos.value['DataEntrada']);
//     Output.NewRecord;
//     Output.SetFieldByName('Apontamento',Apontamentos.Value['Apontamento']);
//     Output.SetFieldByName('ApontamentoDetalhado',Apontamentos.Value['ApontamentoDetalhado']);
//     Output.SetFieldByName('Horario',Apontamentos.Value['Horario']);
//     if TotalHoraCalculada <= ValorRepasse.LimiteHorasTrabalhadas then
//       Output.SetFieldByName('ValorRepasse', ValorRepasse.Normal * Apontamentos.Value['Tempo'])
//    else
//      Output.SetFieldByName('ValorRepasse', ValorRepasse.Excedente * Apontamentos.Value['Tempo']);
//
//     if TotalHoraCalculada <= ValorCobranca.LimiteHorasTrabalhadas then
//       Output.SetFieldByName('ValorCobranca', ValorCobranca.Normal * Apontamentos.Value['Tempo'])
//     else
//       Output.SetFieldByName('ValorCobranca', ValorCobranca.Excedente * Apontamentos.Value['Tempo']);
//
//     Output.SetFieldByName('Descricao','Apontamento');
//     Output.SetFieldByName('PeriodoCobranca',ValorCobranca.Periodo);
//     Output.SetFieldByName('PeriodoRepasse' ,ValorRepasse.Periodo);
//
//     Cooperado := Apontamentos.value['Cooperado'];
//     DataEntrada := Apontamentos.value['DataEntrada'];  
//   // VeiculoCobranca
//   // VeiculoRepasse
//    Apontamentos.Next;
//  end;
//end;

procedure CalcularContratoMes(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
  C: IwtsCommand;
  Contrato, CondicaoComercialCooperado, Cooperados,
  Fechamento,Extrato, Periodos: IwtsWriteData;
  TotalHoras, TotalIntervalos: Double;
begin
  C := DataPool.Open('MILLENIUM');

  Contrato := Input.AsData['Contrato'];
  CondicaoComercialCooperado := Input.AsData['CondicaoComercialCooperado'];
  Cooperados := Input.AsData['Cooperados'];

  while not Cooperados.EOF do
  begin
    Extrato := Cooperados.AsData['Extrato'] as IwtsWriteData;
    Extrato.First;
    TotalIntervalos := 0;
    TotalHoras := 0;
    while not Extrato.EOF do
    begin

      TotalIntervalos := TotalIntervalos + HoraToDecimal(Extrato.Value['Intervalo']);
      TotalHoras := TotalHoras + HoraToDecimal(Extrato.Value['Total']);
    end;



    // Cooperados.fil


    Output.NewRecord;
    Output.Values['Cooperado'] := Cooperados.Value['Cooperado'];
    Output.Values['Descricao'] := Cooperados.Value['Cooperado'];
    Cooperados.Next;
  end;
//
//  OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULO.PERIODO
//
//
//
//
//
//
//
//    Periodos
//      Periodo
//      Horas
//      Valor
//    Periodos
//      Periodo
//      Horas
//      Valor

end;

procedure CalcularContratoDia(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
begin

end;

procedure CalcularContratoHora(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
begin

end;

initialization
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.FecharContrato', FecharContrato);
  //wtsRegisterProc('CONTRATOS.FECHAMENTOS.CalcularContrato', CalcularContrato);
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.CalcularContratoMes', CalcularContratoMes);
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.CalcularContratoDia', CalcularContratoDia);
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.CalcularContratoHora', CalcularContratoHora);





end.
