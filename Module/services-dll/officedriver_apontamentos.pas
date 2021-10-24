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
    Output.NewRecord;
    Output.Values['Horario'] := I;
    Output.Values['Tempo'] := 1;

    if (I = HoraInicio) and (MinutoInicio <> 0) then
      Output.Values['Tempo'] := MinutoInicio/60;

    if (I = HoraFim) and (MinutoFim <> 0) then
      Output.Values['Tempo'] := MinutoFim/60;

  end;
end;


initialization
  wtsRegisterProc('APONTAMENTOS.Detalhar', Detalhar);

end.
