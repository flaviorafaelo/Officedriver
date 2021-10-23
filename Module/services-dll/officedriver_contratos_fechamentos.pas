unit officedriver_contratos_fechamentos;

interface

uses
  wtsServerObjs;

implementation

procedure FecharContrato(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   C: IwtsCommand;
   ApontamentosCalculados: IwtsData;
begin
  C := DataPool.Open('OFFICEDRIVER');
  C.Execute('#CALL OFFICEDRIVER.CONTRATOS.FECHAMENTOS.CALCULARCONTRATO(:Fechamento,:Contrato)');
  ApontamentosCalculados := C.CreateRecordset;
  while not ApontamentosCalculados.EOF do
  begin
     C.Dim('Fechamento',Input.Value['Fechamento']);
     C.Dim('Contrato',Input.Value['Contrato']);
     C.Dim('Apontamento', ApontamentosCalculados.Value['Apontamento']);
     C.Dim('ValorRepasse', ApontamentosCalculados.Value['ValorRepasse']);
     C.Dim('ValorCobranca', ApontamentosCalculados.Value['ValorCobranca']);
     C.Execute('INSERT INTO FechamentosApontamentos(Fechamento,Contrato,Apontamento,ValorRepasse,ValorCobranca) VALUES (:Fechamento,:Contrato,:Apontamento,:ValorRepasse,:ValorCobranca) #RETURN(ID)');

     ApontamentosCalculados.Next;
  end;
end;

procedure CalcularContrato(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   C: IwtsCommand;
   Fechamento,Contrato,ContratoCooperados,ContratoCondicoesComerciais, Apontamentos: IwtsData;
begin
  C := DataPool.Open('OFFICEDRIVER');

  C.Dim('Fechamento', Input.Value['Fechamento']);
  C.Execute('SELECT * FROM Fechamentos WHERE Id =:Fechamento');
  Fechamento := C.CreateRecordset;

  C.Dim('Contrato', Input.Value['Contraro']);
  C.Execute('SELECT * FROM Contratos WHERE Contrato =:Contrato');
  Contrato := C.CreateRecordset;

  C.Dim('Contrato', Input.Value['Contraro']);
  C.Execute('SELECT * FROM ContratosCooperados WHERE Contrato =:Contrato');
  ContratoCooperados := C.CreateRecordset;

  C.Dim('Contrato', Input.Value['Contraro']);
  C.Execute('SELECT * FROM ContratosCooperados WHERE Contrato =:Contrato');
  ContratoCondicoesComerciais := C.CreateRecordset;




  C.Dim('Contrato', Input.Value['Contrato']);
  C.Dim('DataBaseFechamento', Fechamento.Value['DataBaseFechamento']);
  C.Execute('SELECT * FROM Apontamentos WHERE Contrato =:Contrato AND Fechamento IS NULL and DataEntrada <= :DataBaseFechamento ORDER BY Cooperado');
  Apontamentos := C.CreateRecordset;

  while not Apontamentos.EOF do
  begin
     //Funcao :=
   // Apontamento
  //  ValorCobranca
  //  ValorRepasse
    Apontamentos.Next;
  end;

end;



initialization
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.FecharContrato', FecharContrato);
  wtsRegisterProc('CONTRATOS.FECHAMENTOS.CalcularContrato', CalcularContrato);

end.
