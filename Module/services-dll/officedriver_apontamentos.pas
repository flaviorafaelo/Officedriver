unit officedriver_apontamentos;

interface

uses
  SysUtils, DateUtils, wtsServerObjs, officedriver_utils, Math;

implementation

procedure Detalhar(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   Entrada, Saida, Data, Horario: TDateTime;
   HoraInicio,MinutoInicio,
   HoraFim,MinutoFim, Hora: Integer;

   Tempo, diferenca : Double;

  procedure SepararHora(AHorario: string; var AHora: Integer; var AMinutos: Integer);
  begin
    AHora := StrToInt(Copy(AHorario, 1,2));
    AMinutos := StrToInt(Copy(AHorario, 4,2));
  end;

begin
  Entrada := Input.Value['Entrada'];
  Saida := Input.Value['Saida'];
  Data := Entrada;

  LogDebug('--------------------------------------------------------------------','Extrato');
  LogDebug('Entrada: '+FormatDateTime('dd/mm/yyyy hh:nn',Entrada) +
           ' Saida: '+FormatDateTime('dd/mm/yyyy hh:nn',Saida), 'Extrato');
  LogDebug('--------------------------------------------------------------------','Extrato');

  if Saida < Entrada then
    Saida := IncDay(Saida);

  SepararHora(FormatDateTime('hh:nn',Entrada), HoraInicio, MinutoInicio);
  SepararHora(FormatDateTime('hh:nn',Saida)  , HoraFim, MinutoFim);

  Diferenca := MinuteSpan(Saida, Entrada);
  while (Diferenca > 0) do
  begin

    Hora := StrToInt(FormatDateTime('hh', Data));
    if (Hora = HoraInicio) and (MinutoInicio <> 0) then
    begin
      Horario := Entrada;
      Tempo := (60 - MinutoInicio)/60;
      Data := IncMinute(Data, 60 - MinutoInicio);
    end else
    if (Hora = HoraFim) and (MinutoFim <> 0) then
    begin
       Horario := Saida;
       Tempo := MinutoFim/60;
       Data :=  Saida;
    end else
    begin
      Horario := Data;
      Tempo := 1;
      Data := IncHour(Data, 1);
    end;
    Output.NewRecord;
    Output.Values['Horario'] := Horario;
    Output.Values['Tempo'] := Tempo;
    Diferenca := Round(Diferenca - (Tempo * 60));
    LogDebug(FormatDateTime('dd/mm/yyyy hh:nn',Horario) + ': '+FloatToStr(Tempo), 'Extrato');
  end;
  LogDebug('--------------------------------------------------------------------','Extrato');
end;


initialization
  wtsRegisterProc('APONTAMENTOS.Detalhar', Detalhar);

end.
