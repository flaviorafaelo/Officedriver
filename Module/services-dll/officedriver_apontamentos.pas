unit officedriver_apontamentos;

interface

uses
  SysUtils, wtsServerObjs;

implementation

procedure Detalhar(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   HorarioInicial,HorarioFinal: string;

   HoraInicio,MinutoInicio,
   HoraFim,MinutoFim: Integer;
   I: Integer;
   procedure SepararHora(AHorario: string; var AHora: Integer; var AMinutos: Integer);
   begin
     AHora := StrToInt(Copy(AHorario, 1,2));
     AMinutos := StrToInt(Copy(AHorario, 4,2));
   end;
begin
  HorarioInicial := Input.AsString['HorarioInicial'];
  HorarioFinal := Input.AsString['HorarioFinal'];

  if Length(HorarioInicial) < 5 then
    raise Exception.Create('Horario Inicial não está no formato esperado. 00:00');

  if Length(HorarioFinal) < 5 then
    raise Exception.Create('Horario Final não está no formato esperado. 00:00');

  SepararHora(HorarioInicial, HoraInicio, MinutoInicio);
  SepararHora(HorarioFinal, HoraFim, MinutoFim);

  for I := HoraInicio to HoraFim  do
  begin
    if (I = HoraInicio) and (MinutoInicio <> 0) then
    begin
      Output.NewRecord;
      Output.Values['Horario'] := HorarioInicial;
      Output.Values['Tempo'] := MinutoInicio/60;
    end else
    if (I = HoraFim) then
    begin
      if (MinutoFim <> 0) then
      begin
        Output.NewRecord;
        Output.Values['Horario'] := HorarioFinal;
        Output.Values['Tempo'] := MinutoFim/60;
      end;
    end else
    begin
      Output.NewRecord;
      Output.Values['Horario'] := FormatFloat('00', I)+':00';
      Output.Values['Tempo'] := 1;
    end;
  end;
end;


initialization
  wtsRegisterProc('APONTAMENTOS.Detalhar', Detalhar);

end.
