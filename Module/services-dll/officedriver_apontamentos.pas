unit officedriver_apontamentos;

interface

uses
  SysUtils, DateUtils, wtsServerObjs,Windows, logfiles;

implementation


procedure Log(msg: string; prefix: string = 'Calculo');
begin
  AddLog(0,msg, 'Calculo');
end;

procedure LogDebug(msg: string; prefix: string = 'Calculo');
begin
  {$IFDEF MSWINDOWS}
  if Length(msg) > 0 then OutputDebugString(PChar('Calculo:> ' + msg));
  Log(msg, prefix);
  {$ENDIF}
end;

procedure Detalhar(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   Entrada, Saida, Data: TDateTime;
   HoraInicio,MinutoInicio,
   HoraFim,MinutoFim, Hora: Integer;

   Horario: string;
   Tempo: Double;

  procedure SepararHora(AHorario: string; var AHora: Integer; var AMinutos: Integer);
  begin
    AHora := StrToInt(Copy(AHorario, 1,2));
    AMinutos := StrToInt(Copy(AHorario, 4,2));
  end;

begin
  Entrada := Input.Value['Entrada'];
  Saida := Input.Value['Saida'];

  LogDebug('--------------------------------------------------------------------','Extrato');
  LogDebug('Entrada: '+FormatDateTime('dd/mm/yyyy hh:nn',Entrada) +
           ' Saida: '+FormatDateTime('dd/mm/yyyy hh:nn',Saida));
  LogDebug('--------------------------------------------------------------------','Extrato');

  if Saida < Entrada then
    Saida := IncDay(Saida);

  SepararHora(FormatDateTime('hh:nn',Entrada), HoraInicio, MinutoInicio);
  SepararHora(FormatDateTime('hh:nn',Saida)  , HoraFim, MinutoFim);

  Data := Entrada;
  while (Data < Saida) do
  begin
    Hora := StrToInt(FormatDateTime('hh', Data));
    if (Hora = HoraInicio) and (MinutoInicio <> 0) then
    begin
      Horario := FormatDateTime('hh:nn',Entrada);
      Tempo := (60 - MinutoInicio)/60;
      Data := IncMinute(Data, 60 - MinutoInicio);
    end else
    if (Hora = HoraFim) and (MinutoFim <> 0) then
    begin
       Horario := FormatDateTime('hh:nn',Saida);
       Tempo := MinutoFim/60;
       Data :=  Saida;
    end else
    begin
      Horario := FormatDateTime('hh:nn',Data);
      Tempo := 1;
      Data := IncHour(Data, 1);
    end;
    Output.NewRecord;
    Output.Values['Horario'] := Horario;
    Output.Values['Tempo'] := Tempo;

    LogDebug(Horario + ': '+FloatToStr(Tempo));
  end;
  LogDebug('--------------------------------------------------------------------','Extrato');
end;


initialization
  wtsRegisterProc('APONTAMENTOS.Detalhar', Detalhar);

end.
