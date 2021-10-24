unit officedriver_contratos_fechamentos;

interface

uses
  wtsServerObjs, SysUtils;
Type
  TValores = record
    Normal,
    Excedente: Double;
    LimiteHorasTrabalhadas: Integer;
  end;


implementation

procedure FecharContrato(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   C: IwtsCommand;
   ApontamentosCalculados: IwtsData;
begin
  C := DataPool.Open('MILLENIUM');   
  C.Execute('#CALL OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULARCONTRATO(:Fechamento,:Contrato)');
  ApontamentosCalculados := C.CreateRecordset;

  if ApontamentosCalculados.EOF then
    raise Exception.Create('Não existem apontamentos em aberto para data selecionada.');
  
  
  while not ApontamentosCalculados.EOF do
  begin
     C.Dim('Fechamento',Input.Value['Fechamento']);
     C.Dim('Apontamento', ApontamentosCalculados.Value['Apontamento']);
     C.Dim('ValorRepasse', ApontamentosCalculados.Value['ValorRepasse']);
     C.Dim('ValorCobranca', ApontamentosCalculados.Value['ValorCobranca']);
     C.Dim('Horario', ApontamentosCalculados.Value['Horario']);
     C.Execute('INSERT INTO FechamentosApontamentos(Fechamento,Apontamento,ValorRepasse,ValorCobranca, Horario) VALUES (:Fechamento,:Apontamento,:ValorRepasse,:ValorCobranca,:Horario) #RETURN(ID)');

     C.Dim('Fechamento',Input.Value['Fechamento']);
     C.Dim('Apontamento', ApontamentosCalculados.Value['Apontamento']);
     C.Execute('Update Apontamentos set Fechamento = :Fechamento WHERE Id = :Apontamento;');

     ApontamentosCalculados.Next;                                                
  end;

  C.Execute('Update FechamentosContratos set ValorCobranca = (SELECT SUM(ValorCobranca) FROM FechamentosApontamentos WHERE Fechamento = :Fechamento), '+
            '                               ValorRepasse = (SELECT SUM(ValorRepasse) FROM FechamentosApontamentos WHERE Fechamento = :Fechamento) '+
            'WHERE Fechamento = :Fechamento and Contrato = :Contrato;');
  
end;

procedure CalcularContrato(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   C: IwtsCommand;
   Fechamento,Contrato,Apontamentos,ApontamentosDetalhado,CondicoesCobranca,
   CondicoesRepasse,Cooperados: IwtsWriteData;
   ValorCobranca, ValorRepasse: TValores;
   TotalHoraCalculada: Double;
   DataEntrada: TDateTime;
   Cooperado: Integer;
   procedure PreparaCondicao;
   begin

   end;

   function ObterValorHora(const AData: IwtsWriteData; const AHora: string; AFuncao: Integer): TValores;
   begin
     AData.First;
     while not AData.Eof do
     begin
       if (AData.Value['Funcao'] = AFuncao) and
          (AHora >= AData.Value['Inicio']) and (AHora <= AData.Value['Fim']) then
       begin
         Result.Normal := AData.Value['ValorNormal'];
         Result.Excedente := AData.Value['ValorExcedente'];
         Result.LimiteHorasTrabalhadas := AData.Value['Horas'];
         Exit;
       end;
       AData.Next;
     end;
   end;
begin
  C := DataPool.Open('MILLENIUM');

  C.Dim('Fechamento', Input.Value['Fechamento']);
  C.Execute('SELECT * FROM Fechamentos WHERE Id =:Fechamento');
  Fechamento := C.CreateRecordset;

  C.Dim('Contrato', Input.Value['Contrato']);
  C.Execute('SELECT * FROM Contratos WHERE Id =:Contrato');
  Contrato := C.CreateRecordset;

  C.Dim('Contrato', Input.Value['Contrato']);
  C.Execute('SELECT * FROM ContratosCooperados WHERE Contrato =:Contrato');
  Cooperados := C.CreateRecordset;

  C.Dim('Contrato', Input.Value['Contrato']);
  C.Execute('SELECT * '+
            ' FROM ContratosCondComercialCoop ccc '+ 
            ' INNER JOIN CondicaoComPeriodo ccp ' +
            ' ON ccp.condicao = ccc.id '+
            ' WHERE ccc.Contrato =:Contrato and ccc.Tipo = "C"');
  CondicoesCobranca := C.CreateRecordset;

  C.Dim('Contrato', Input.Value['Contrato']);
  C.Execute('SELECT * '+
            ' FROM ContratosCondComercialCoop ccc '+ 
            ' INNER JOIN CondicaoComPeriodo ccp ' +
            ' ON ccp.condicao = ccc.id '+
            ' WHERE ccc.Contrato =:Contrato and ccc.Tipo = "R"');
  CondicoesRepasse := C.CreateRecordset;

  C.Dim('Contrato', Input.Value['Contrato']);
  C.Dim('DataBaseFechamento', Fechamento.Value['DataBaseFechamento']);
  C.Execute('SELECT Ap.Id, Ap.DataEntrada, Ad.Horario, Ad.Tempo, Ap.Cooperado FROM ApontamentosDetalhados Ad Inner Join Apontamentos Ap   '+
            'ON Ad.Apontamento = Ap.Id '+
            ' WHERE Ap.Contrato =:Contrato AND Ap.Fechamento IS NULL and Ap.DataEntrada <= :DataBaseFechamento '+
            ' Order by Ap.Cooperado');
  Apontamentos := C.CreateRecordset;

  TotalHoraCalculada := 0;

  while not Apontamentos.EOF do
  begin
     Cooperados.Locate(['Cooperado'],[Apontamentos.Value['Cooperado']]);

     if (Apontamentos.value['DataEntrada'] <> DataEntrada) or (Apontamentos.value['Cooperado'] <> Cooperado)  then
      TotalHoraCalculada := 0;
     
     TotalHoraCalculada := TotalHoraCalculada + Apontamentos.Value['Tempo'];
     ValorCobranca := ObterValorHora(CondicoesCobranca, Apontamentos.Value['Horario'], Cooperados.Value['Funcao']);
     ValorRepasse  := ObterValorHora(CondicoesRepasse , Apontamentos.Value['Horario'], Cooperados.Value['Funcao']);
     Output.NewRecord;
     Output.SetFieldByName('Apontamento',Apontamentos.Value['Id']);
     Output.SetFieldByName('Horario',Apontamentos.Value['Horario']);
     if TotalHoraCalculada <= ValorRepasse.LimiteHorasTrabalhadas then
       Output.SetFieldByName('ValorRepasse', ValorRepasse.Normal * Apontamentos.Value['Tempo'])
    else  
      Output.SetFieldByName('ValorRepasse', ValorRepasse.Excedente * Apontamentos.Value['Tempo']);
 
     if TotalHoraCalculada <= ValorCobranca.LimiteHorasTrabalhadas then
       Output.SetFieldByName('ValorCobranca', ValorCobranca.Normal * Apontamentos.Value['Tempo'])
     else
       Output.SetFieldByName('ValorCobranca', ValorCobranca.Excedente * Apontamentos.Value['Tempo']);
     Cooperado := Apontamentos.value['Cooperado'];
     DataEntrada := Apontamentos.value['DataEntrada'];  
   // VeiculoCobranca
   // VeiculoRepasse
    Apontamentos.Next;
  end;

end;



initialization
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.FecharContrato', FecharContrato);
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.CalcularContrato', CalcularContrato);

end.
