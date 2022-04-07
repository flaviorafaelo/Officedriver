unit officedriver_contratos_fechamentos;

interface

uses
  wtsServerObjs, SysUtils, DateUtils, officedriver_utils, Variants;

type
  TTipo = (tRepasse, tCobranca);
  TTipoContrato = (ttFixo, ttVariavel);
  TBaseCaculo = (bcMensal, bcDiario, bcHora);
  TTipoPeriodo = (tpNormal = 0, tpFds = 1, tpFeriado = 2);

Type
  TValorPeriodo = record
    Descricao: String;
    Normal,
    Excedente,
    ValorHoraNormal,
    ValorHoraExcedente: Double;
    Tipo: Integer;
    Econtrado: Boolean;
  end;

implementation

procedure CalcularPeriodo(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
  C: IwtsCommand;
  Periodos, Detalhe, Detalhe1, Detalhe2, Feriados: IwtsWriteData;
  Entrada, Saida, EntradaAux, SaidaAux, InicioIntervalo, FimIntervalo, HoraPeriodo : TDateTime;

  ValorPeriodo: TValorPeriodo;
  InicioIntervaloS,FimIntervaloS: string;

  function ObterValorPeriodo(const AData: IwtsWriteData; const AHora: TDateTime;
     AEntrada, ASaida: TDateTime): TValorPeriodo;
  var Tipo: TTipoPeriodo;
      HoraInicio, HoraFim, Hora: TDateTime;
  begin
    Tipo := tpNormal;

    if Feriados.Locate(['Data'],[AEntrada]) then
     Tipo := tpFeriado
    else if (DayOfWeek(AEntrada) = 1) or (DayOfWeek(AEntrada) = 7) then
      Tipo := tpFds;

    AData.First;
    Result.Normal    := 0;
    Result.Excedente := 0;
    Result.ValorHoraNormal := 0;
    Result.ValorHoraExcedente := 0;
    Result.Tipo      := 0;
    Result.Descricao := '';
    Result.Econtrado := False;

    while not AData.Eof do
    begin
      HoraInicio := StrToDateTime(DateToStr(AEntrada) +' ' + AData.Value['Inicio']);
      HoraFim := StrToDateTime(DateToStr(ASaida)+' ' +AData.Value['Fim']);
      Hora := AHora;

      if (HoraFim < HoraInicio) then
        HoraFim := IncDay(HoraFim);

      LogDebug('Hora: '+ FormatDateTime('dd/mm/yyyy hh:mm', aHora)+' Inicio: '+FormatDateTime('dd/mm/yyyy hh:nn', HoraInicio) + ' Fim: ' + FormatDateTime('dd/mm/yyyy hh:nn', HoraFim),'');

      if (Hora >= HoraInicio) and (Hora < HoraFim) and
         ((AData.Value['Tipo'] = Tipo) or (AData.Value['Tipo'] = tpNormal)) then
      begin
       Result.Descricao      := AData.Value['Descricao'];
       Result.Normal         := AData.Value['ValorNormal'];
       Result.Excedente      := AData.Value['ValorExcedente'];
       Result.Tipo           := AData.Value['Tipo'];
       Result.ValorHoraNormal:= AData.Value['ValorHora'];
       Result.ValorHoraExcedente:= AData.Value['ValorHoraExcedente'];
       Result.Econtrado := True;
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
  InicioIntervaloS := Input.Value['InicioIntervalo'];
  FimIntervaloS := Input.Value['FimIntervalo'];

  if InicioIntervaloS <> FimIntervaloS then
  begin
    InicioIntervalo := StrToDateTime(FormatDateTime('dd/mm/yyyy',Entrada) + InicioIntervaloS);
    FimIntervalo := StrToDateTime(FormatDateTime('dd/mm/yyyy',Entrada) + FimIntervaloS);

    if InicioIntervalo < Entrada  then
      InicioIntervalo := IncDay(InicioIntervalo);
    if FimIntervalo < InicioIntervalo  then
      FimIntervalo := IncDay(FimIntervalo);
  end;

  C.Dim('AnoInicio', YearOf(Entrada));
  C.Dim('AnoFim', YearOf(Saida));
  C.Execute('SELECT Data FROM Feriados where #YEAR(Data) between :AnoInicio and :AnoFim');
  Feriados := C.CreateRecordset;

  if InicioIntervaloS <> FimIntervaloS then
  begin
    C.Dim('Entrada',Entrada);
    C.Dim('Saida',InicioIntervalo);
    C.Execute('#CALL OFFICEDRIVER.APONTAMENTOS.Detalhar(:Entrada,:Saida);');
    Detalhe1 := C.CreateRecordset;

    C.Dim('Entrada',FimIntervalo);
    C.Dim('Saida',Saida);
    C.Execute('#CALL OFFICEDRIVER.APONTAMENTOS.Detalhar(:Entrada,:Saida);');
    Detalhe2 := C.CreateRecordset;

    Detalhe := DataPool.CreateRecordset('OFFICEDRIVER.APONTAMENTOS.Detalhar',True);
    Detalhe.CopyFrom(Detalhe1);
    Detalhe.CopyFrom(Detalhe2);
  end else
  begin
    C.Dim('Entrada',Entrada);
    C.Dim('Saida',Saida);
    C.Execute('#CALL OFFICEDRIVER.APONTAMENTOS.Detalhar(:Entrada,:Saida);');
    Detalhe := C.CreateRecordset;
  end;

  EntradaAux := 0;
  SaidaAux := 0;
  Detalhe.First;

  if DayOf(Saida) <> DayOf(Entrada) then
  begin
    EntradaAux := StrToDateTime(FormatDateTime('dd/mm/yyyy', Saida) + '00:00');
    SaidaAux := StrToDateTime(FormatDateTime('dd/mm/yyyy', Entrada) + '23:59:59');
  end;

  if EntradaAux = 0 then
  begin
    while not Detalhe.EOF do
    begin
      LogDebug(Detalhe.Value['Horario']);
      HoraPeriodo := Detalhe.Value['Horario'];

      ValorPeriodo := ObterValorPeriodo(Periodos, HoraPeriodo, Entrada, Saida);
      if ValorPeriodo.Econtrado then
      begin
       Output.NewRecord;
       Output.SetFieldByName('Descricao',ValorPeriodo.Descricao);
       Output.SetFieldByName('Horas',DecimalToHour(Detalhe.Value['Tempo']));
       Output.SetFieldByName('Valor',Detalhe.Value['Tempo'] * ValorPeriodo.Normal);
       Output.SetFieldByName('Tipo',ValorPeriodo.Tipo);
       Output.SetFieldByName('ValorHora',ValorPeriodo.Normal);
       Output.SetFieldByName('ValorHoraExcedente',ValorPeriodo.Excedente);
      end;
      Detalhe.Next;
    end;
  end else
  begin
    while not Detalhe.EOF do
    begin
      LogDebug(Detalhe.Value['Horario']);
      HoraPeriodo := Detalhe.Value['Horario'];

      ValorPeriodo := ObterValorPeriodo(Periodos, HoraPeriodo, Entrada, SaidaAux);
      if ValorPeriodo.Econtrado then
      begin
       Output.NewRecord;
       Output.SetFieldByName('Descricao',ValorPeriodo.Descricao);
       Output.SetFieldByName('Horas',DecimalToHour(Detalhe.Value['Tempo']));
       Output.SetFieldByName('Valor',Detalhe.Value['Tempo'] * ValorPeriodo.Normal);
       Output.SetFieldByName('Tipo',ValorPeriodo.Tipo);
       Output.SetFieldByName('ValorHora',ValorPeriodo.Normal);
       Output.SetFieldByName('ValorHoraExcedente',ValorPeriodo.Excedente);
      end;
      Detalhe.Next;
    end;
    Detalhe.First;
    while not Detalhe.EOF do
    begin
      LogDebug(Detalhe.Value['Horario']);
      HoraPeriodo := Detalhe.Value['Horario'];

      ValorPeriodo := ObterValorPeriodo(Periodos, HoraPeriodo, EntradaAux, Saida);
      if ValorPeriodo.Econtrado then
      begin
       Output.NewRecord;
       Output.SetFieldByName('Descricao',ValorPeriodo.Descricao);
       Output.SetFieldByName('Horas',DecimalToHour(Detalhe.Value['Tempo']));
       Output.SetFieldByName('Valor',Detalhe.Value['Tempo'] * ValorPeriodo.Normal);
       Output.SetFieldByName('Tipo',ValorPeriodo.Tipo);
       Output.SetFieldByName('ValorHora',ValorPeriodo.Normal);
       Output.SetFieldByName('ValorHoraExcedente',ValorPeriodo.Excedente);
      end;
      Detalhe.Next;
    end;
  end;
end;

procedure FecharContrato(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   C: IwtsCommand;
   ApontamentosCalculados: IwtsData;
   Evento, TipoContrato, ValorContrato: Integer;
   Data, DataFechamento: TDateTime;
   BaseCalculo: TBaseCaculo;
   Contrato, CondicoesComerciaisCooperado, CondicaoComercial,Apontamentos,DetalheApontamento,
   CooperadosContratos, CalculoContratos, Cooperados, CooperadosOut, Extrato, ExtratosOut, TotalOut, PeriodosOut: IwtsWriteData;
   i, IdContrato, IdFechamentoExtrato: Integer;
   Intervalo, TotalGeralIntervalo, TotalGeral, TotalGeralExcedente, ValorTotalGeral, ValorTotalGeralExcedente: Double;
   Total :String;
begin
  C := DataPool.Open('MILLENIUM');
  DataFechamento := IncDay(Input['DataBaseFechamento'],-1);

  Contrato := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.CONTRATO');
  Cooperados := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULO.COOPERADO');
  CalculoContratos := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULARCONTRATOMES');
  CondicaoComercial := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.CONDICAOCOMERCIALCOOPERADO');
  Extrato := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULO.EXTRATO');

  C.Execute('#CALL OFFICEDRIVER.CONTRATOS.Consultar(:Contrato)');
  Contrato.CopyFrom(C);

  CondicoesComerciaisCooperado := Contrato.AsData['CondComCooperado'] as IwtsWriteData;
  LogDebug(CondicoesComerciaisCooperado.IntfName);
  LogDebug(CondicaoComercial.IntfName);
  while not CondicoesComerciaisCooperado.EOF do
  begin
    BaseCalculo := CondicoesComerciaisCooperado.Value['BASECALCULO'];

    CondicaoComercial.Clear;
    CondicaoComercial.New;
    for i := 0 to CondicoesComerciaisCooperado.FieldCount - 1 do
      CondicaoComercial.AtIndex[i] := CondicoesComerciaisCooperado.AtIndex[i];
    CondicaoComercial.Add;

    C.Dim('Fechamento',Input.Value['Fechamento']);
    C.Dim('Contrato',Input.Value['Contrato']);
    C.Dim('Funcao',CondicaoComercial.Value['Funcao']);
    C.Execute('#CALL OFFICEDRIVER.CONTRATOS.FECHAMENTOS.ListarApontamentos(:Fechamento,:Contrato,:Funcao);');
    Apontamentos := C.AsData['Apontamentos'] as IwtsWriteData;

    //Montagem do Extrado
    CooperadosContratos := Contrato.AsData['Cooperados'] as IwtsWriteData;
    CooperadosContratos.Filter := 'FUNCAO='+CondicoesComerciaisCooperado.GetFieldAsString('Funcao');
    while not CooperadosContratos.EOF do
    begin
      Data := IncMonth(DataFechamento, -1);
      Data := IncDay(Data, 1);
      Extrato.Clear;
      while Data <= DataFechamento do
      begin
         Log(CooperadosContratos.GetFieldAsString('Cooperado') + ' - '+FormatDateTime('dd/mm/yyyy hh:nn',Data));
        if Apontamentos.Locate(['Cooperado', 'DataEntrada'],[CooperadosContratos.Value['Cooperado'], Data]) then
        begin
          Extrato.New;
          Extrato.Value['Entrada']          := Apontamentos.Value['Entrada'];
          Extrato.Value['Saida']            := Apontamentos.Value['Saida'];
          Extrato.Value['DataEntrada']      := Apontamentos.Value['DataEntrada'];
          Extrato.Value['DiaSemanaEntrada'] := FormatDateTime('dddd',Apontamentos.Value['Entrada']);
          Extrato.Value['HoraEntrada']      := FormatDateTime('hh:nn',Apontamentos.Value['Entrada']);
          Extrato.Value['DataSaida']        := FormatDateTime('dd/mm/yyyy',Apontamentos.Value['Saida']);
          Extrato.Value['DiaSemanaSaida']   := FormatDateTime('dddd',Apontamentos.Value['Saida']);
          Extrato.Value['HoraSaida']        := FormatDateTime('hh:nn',Apontamentos.Value['Saida']);
          Extrato.Value['Trabalhada']       := FormatDateTime('hh:nn',Apontamentos.Value['Saida'] - Apontamentos.Value['Entrada']);
          Extrato.Value['InicioIntervalo']  := Apontamentos.Value['InicioIntervalo'];
          Extrato.Value['FimIntervalo']     := Apontamentos.Value['FimIntervalo'];
          Extrato.Value['Total']            := Extrato.Value['Trabalhada'];
          Extrato.Add;
        end else
        begin
//          Extrato.New;
//          Extrato.Value['Entrada']          := 0;
//          Extrato.Value['Saida']            := 0;
//          Extrato.Value['DataEntrada']      := Data;
//          Extrato.Value['DiaSemanaEntrada'] := FormatDateTime('dddd',Data);
//          Extrato.Value['HoraEntrada']      := '00:00';
//          Extrato.Value['DataSaida']        := FormatDateTime('dd/mm/yyyy',Data);
//          Extrato.Value['DiaSemanaSaida']   := FormatDateTime('dddd',Data);
//          Extrato.Value['HoraSaida']        := '00:00';
//          Extrato.Value['Trabalhada']       := '00:00';
//          Extrato.Value['InicioIntervalo']  := '00:00';
//          Extrato.Value['FimIntervalo']     := '00:00';
//          Extrato.Value['Total']            := '00:00';
//          Extrato.Add;
        end;
        Data := IncDay(Data,1);
      end;
      Cooperados.New;
      Cooperados.Value['Cooperado'] := CooperadosContratos.Value['Cooperado'];
      Cooperados.Value['Extrato'] := Extrato.Data;
      Cooperados.Add;
      CooperadosContratos.Next;
    end;

    case BaseCalculo of
      bcMensal:
      begin
        C.DimAsData('Contrato',Contrato);
        C.DimAsData('CondicaoComercialCooperado',CondicaoComercial);
        C.DimAsData('Cooperados',Cooperados);
        C.Execute('#CALL OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CalcularContratoMes(:Contrato,:CondicaoComercialCooperado,:Cooperados)');

        CalculoContratos.CopyFrom(C);

        TotalGeralIntervalo := 0;
        TotalGeral := 0;
        TotalGeralExcedente := 0;
        ValorTotalGeral := 0;
        ValorTotalGeralExcedente := 0;

        CooperadosOut := CalculoContratos.AsData['Cooperados'] as IwtsWriteData;
        CooperadosOut.First;
        while not CooperadosOut.EOF do
        begin
          ExtratosOut := CooperadosOut.AsData['Extrato'] as IwtsWriteData;
          ExtratosOut.First;
          while not ExtratosOut.EOF do
          begin
            Intervalo := HourToDecimal(ExtratosOut.Value['FimIntervalo']) - HourToDecimal(ExtratosOut.Value['InicioIntervalo']);
            LogDebug(FormatDateTime('dd/mm/yyyy',ExtratosOut.Value['DataEntrada']));
            PeriodosOut := ExtratosOut.AsData['Periodos'] as IwtsWriteData;
            C.Dim('Fechamento', Input.Value['Fechamento']);
            C.Dim('Cooperado', CooperadosOut.Value['Cooperado']);
            C.Dim('DataEntrada',ExtratosOut.Value['DataEntrada']);
            C.Dim('DiaSemanaEntrada',ExtratosOut.Value['DiaSemanaEntrada']);
            C.Dim('HoraEntrada',ExtratosOut.Value['HoraEntrada']);
            C.Dim('DataSaida',ExtratosOut.Value['DataSaida']);
            C.Dim('DiaSemanaSaida',ExtratosOut.Value['DiaSemanaSaida']);
            C.Dim('HoraSaida',ExtratosOut.Value['HoraSaida']);
            C.Dim('Trabalhada',ExtratosOut.Value['Trabalhada']);
            C.Dim('Intervalo', DecimalToHour(Intervalo));
            C.Dim('Total',DecimalToHour(HourToDecimal(ExtratosOut.Value['Total']) - Intervalo));
            C.Dim('CondComCoop',CondicoesComerciaisCooperado.Value['Id']);
            C.Execute('Insert into FechamentosExtratos (Fechamento, Cooperado, DataEntrada, ' +
                       'DiaSemanaEntrada, HoraEntrada, DataSaida, DiaSemanaSaida, HoraSaida, '+
                       'Trabalhada, Intervalo, Total, CondComCoop) values  (:Fechamento, :Cooperado, :DataEntrada, ' +
                       ':DiaSemanaEntrada, :HoraEntrada, :DataSaida, :DiaSemanaSaida, :HoraSaida, '+
                       ':Trabalhada, :Intervalo, :Total, :CondComCoop) #RETURN(Id);');
            IdFechamentoExtrato := C.Value['Id'];

            PeriodosOut.First;
            PeriodosOut.Order('Descricao');
            while not PeriodosOut.EOF do
            begin
              C.Dim('Fechamento_Extrato',IdFechamentoExtrato);
              C.Dim('Descricao',PeriodosOut.Value['Descricao']);
              C.Dim('Horas',PeriodosOut.Value['Horas']);
              C.Dim('Valor',PeriodosOut.Value['Valor']);
              C.Dim('Tipo',PeriodosOut.Value['Tipo']);
              C.Dim('ValorHora',PeriodosOut.Value['ValorHora']);
              C.Dim('ValorHoraExcedente',PeriodosOut.Value['ValorHoraExcedente']);
              C.Execute('Insert into FechamentosExtratosPeriodos (Fechamento_Extrato, Descricao, Horas, Valor, Tipo, ValorHora, ValorHoraExcedente) values ' +
              ' (:Fechamento_Extrato, :Descricao, :Horas, :Valor, :Tipo, :ValorHora, :ValorHoraExcedente) #RETURN(Id); ');
              PeriodosOut.Next;
          end;
            ExtratosOut.Next;
          end;

          TotalOut := CooperadosOut.AsData['Total'] as IwtsWriteData;
          while not TotalOut.Eof do
          begin
            TotalGeralIntervalo := TotalGeralIntervalo + HourToDecimal(TotalOut.GetFieldByName('TotalIntervalo'));
            TotalGeral := TotalGeral + HourToDecimal(TotalOut.GetFieldByName('TotalGeral'));
            TotalGeralExcedente := TotalGeralExcedente + HourToDecimal(TotalOut.GetFieldByName('TotalExcedente'));
            ValorTotalGeral := ValorTotalGeral +  TotalOut.GetFieldByName('TotalValor');
            ValorTotalGeralExcedente := ValorTotalGeralExcedente + TotalOut.GetFieldByName('TotalValorExcedente');
            TotalOut.Next;
          end;
          CooperadosOut.Next;
        end;


        C.Dim('Fechamento',Input.Value['Fechamento']);
        C.Dim('TotalIntervalo',DecimalToHour(TotalGeralIntervalo));
        C.Dim('TotalGeral',DecimalToHour(TotalGeral));
        C.Dim('TotalGeralExcedente',DecimalToHour(TotalGeralExcedente));
        C.Dim('ValorTotalGeral',ValorTotalGeral);
        C.Dim('ValorTotalGeralExcedente',ValorTotalGeralExcedente);
        C.Execute('Update Fechamentos set TotalIntervalo = :TotalIntervalo, TotalGeral = :TotalGeral, TotalGeralExcedente = :TotalGeralExcedente, '+
                  ' ValorTotalGeral = :ValorTotalGeral, ValorTotalGeralExcedente = :ValorTotalGeralExcedente '+
                  ' where Id = :Fechamento');
     end;

      bcDiario:
      begin

      end;

      bcHora:
      begin
      end;
    end;

    //Insert


    CondicoesComerciaisCooperado.Next;
  end;


 Log('Finalizado');
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

procedure CalcularContratoMes(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
  C: IwtsCommand;
  Contrato, CondicaoComercial, Cooperados,
  Fechamento,Extrato, Periodos, PeriodosOut, ExtratoPeriodo, Periodo, Total: IwtsWriteData;
  TotalHoras, TotalHorasGeral, TotalHorasExcedente, TotalValor,TotalValorExcedente,
  TotalIntervalos, Intervalo, PacoteHoras, Horas, ValorHoraExcedente, ValorHora: Double;
  LogStr: String;
begin
  C := DataPool.Open('MILLENIUM');
  PeriodosOut := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULO.PERIODOS');
  Periodo := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.PERIODO');
  Total := DataPool.CreateRecordset('OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULO.TOTAL');
  Contrato := Input.AsData['Contrato'];
  CondicaoComercial := Input.AsData['CondicaoComercialCooperado'];
  Cooperados := Input.AsData['Cooperados'] as IwtsWriteData;

  Periodos := CondicaoComercial.AsData['Periodo'] as IwtsWriteData;

  while not Cooperados.EOF do
  begin
    Extrato := Cooperados.AsData['Extrato'] as IwtsWriteData;
    Extrato.First;
    TotalIntervalos := 0;
    TotalHoras := 0;
    TotalHorasExcedente := 0;
    TotalValorExcedente := 0;
    Log('Calcular Periodo');
    while not Extrato.EOF do
    begin
      C.Dim('Entrada', Extrato.Value['Entrada']);
      C.Dim('Saida', Extrato.Value['Saida']);
      C.Dim('InicioIntervalo', Extrato.Value['InicioIntervalo']);
      C.Dim('FimIntervalo', Extrato.Value['FimIntervalo']);
      C.DimAsData('Periodos', Periodos);
      C.Execute('#CALL OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULO.CALCULARPERIODO(Entrada=:Entrada,Saida=:Saida,InicioIntervalo=:InicioIntervalo,FimIntervalo=:FimIntervalo,Periodos=:Periodos);');
      PeriodosOut.Clear;
      while not C.EOF do
      begin
        LogDebug('Descrição: '+C.Value['Descricao'] + ' - Total: ' + C.Value['Horas']);
        PeriodosOut.New;
        PeriodosOut.Value['Descricao']          := C.Value['Descricao'];
        PeriodosOut.Value['Horas']              := C.Value['Horas'];
        PeriodosOut.Value['Valor']              := C.Value['Valor'];
        PeriodosOut.Value['Tipo']               := C.Value['Tipo'];
        PeriodosOut.Value['ValorHora']          := C.Value['ValorHora'];
        PeriodosOut.Value['ValorHoraExcedente'] := C.Value['ValorHoraExcedente'];
        PeriodosOut.Add;
        C.Next;
      end;
      Extrato.Value['Periodos'] := PeriodosOut.Data;
      Extrato.Update;
      Extrato.Next;
    end;

    Cooperados.Value['Extrato'] := Extrato.Data;

    TotalHorasGeral:= 0;
    TotalIntervalos := 0;
    PeriodosOut.Clear;

    //Totalizandio
    TotalHoras := 0;
    TotalValor := 0;
    TotalHorasExcedente := 0;
    Extrato.First;
    PacoteHoras :=  CondicaoComercial.Value['Horas'];

    while not Extrato.EOF do
    begin
      ExtratoPeriodo := Extrato.AsData['Periodos'] as IwtsWriteData;
      ExtratoPeriodo.First;
      Intervalo := HourToDecimal(Extrato.Value['FimIntervalo']) - HourToDecimal(Extrato.Value['InicioIntervalo']);
      while not ExtratoPeriodo.Eof do
      begin
        Horas := HourToDecimal(ExtratoPeriodo.Value['Horas']);
        ValorHora :=  ExtratoPeriodo.Value['Valor'];
        ValorHoraExcedente := ExtratoPeriodo.Value['ValorHoraExcedente'];

        LogStr := ExtratoPeriodo.Value['Descricao'];
        if (TotalHoras < PacoteHoras) and ((TotalHoras + Horas) >= PacoteHoras) then
        begin

          TotalHorasExcedente := TotalHorasExcedente + ((TotalHoras + Horas) - PacoteHoras);
          TotalValorExcedente := TotalValorExcedente + (((TotalHoras + Horas) - PacoteHoras) * ValorHoraExcedente);
          TotalValor :=  TotalValor + ((PacoteHoras - TotalHoras) *  ValorHora);
          TotalHoras := PacoteHoras;
          LogStr := LogStr + ';' + FloatToStr((TotalHoras + Horas) - PacoteHoras) + ';' + FloatToStr(((TotalHoras + Horas) - PacoteHoras) * ValorHoraExcedente) + ';Excedente';
        end else
        if TotalHoras  >= PacoteHoras then
        begin
          TotalHorasExcedente := TotalHorasExcedente + Horas;
          TotalValorExcedente := TotalValorExcedente + (Horas * ValorHoraExcedente);
          LogStr := LogStr + ';' + FloatToStr(Horas) + ';' + FloatToStr(Horas * ValorHoraExcedente) + ';Excedente';
        end else
        begin
          TotalHoras := TotalHoras + Horas;
          TotalValor := TotalValor + ValorHora;
          LogStr := LogStr + ';' + FloatToStr(Horas) + ';' + FloatToStr(Horas * ValorHora) + ';Normal';
        end;
        LogDebug(LogStr);
        ExtratoPeriodo.Next;
      end;
      TotalIntervalos := TotalIntervalos +  Intervalo;
      Extrato.Next;
    end;

    if TotalHoras > 0 then
    begin
      PeriodosOut.New;
      PeriodosOut.Value['Descricao'] := ExtratoPeriodo.Value['Descricao'];
      PeriodosOut.Value['Horas'] := DecimalToHour(TotalHoras);
      PeriodosOut.Value['Valor'] := TotalValor;
      PeriodosOut.Value['Tipo'] := ExtratoPeriodo.Value['Tipo'];
      PeriodosOut.Value['ValorHora'] := ExtratoPeriodo.Value['ValorHora'];
      PeriodosOut.Value['ValorHoraExcedente'] := ExtratoPeriodo.Value['ValorHoraExcedente'];
      PeriodosOut.Add;

      Total.New;
      Total.Value['TotalPeriodo'] := PeriodosOut.Data;
      Total.Value['TotalIntervalo'] := DecimalToHour(TotalIntervalos);
      Total.Value['TotalExcedente'] := DecimalToHour(TotalHorasExcedente);
      Total.Value['TotalGeral'] := DecimalToHour(TotalHoras);
      Total.Value['TotalValor'] := TotalValor;
      Total.Value['TotalValorExcedente'] := TotalValorExcedente;
      Total.Add;

      Cooperados.Value['Total'] :=  Total.Data;
      Cooperados.Update;
    end;
    Cooperados.Next;
  end;
  Output.NewRecord;
  Output.Values['Cooperados'] := Cooperados.Data;
end;

procedure CalcularContratoDia(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
begin

end;

procedure CalcularContratoHora(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
begin

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

initialization
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.FecharContrato', FecharContrato);
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.CALCULO.CalcularPeriodo', CalcularPeriodo);
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.CalcularContratoMes', CalcularContratoMes);
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.CalcularContratoDia', CalcularContratoDia);
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.CalcularContratoHora', CalcularContratoHora);





end.
