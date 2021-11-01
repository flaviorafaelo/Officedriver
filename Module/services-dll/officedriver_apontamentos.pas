unit officedriver_apontamentos;

interface

uses
  SysUtils, DateUtils, wtsServerObjs;

implementation

procedure Detalhar(Input:IwtsInput; Output:IwtsOutput;DataPool:IwtsDataPool);
var
   Entrada, Saida, Data: TDateTime;
   HoraInicio,MinutoInicio,
   HoraFim,MinutoFim, Hora: Integer;
  procedure SepararHora(AHorario: string; var AHora: Integer; var AMinutos: Integer);
  begin
    AHora := StrToInt(Copy(AHorario, 1,2));
    AMinutos := StrToInt(Copy(AHorario, 4,2));
  end;

begin
  Entrada := Input.Value['Entrada'];
  Saida := Input.Value['Saida'];

  SepararHora(FormatDateTime('hh:nn',Entrada), HoraInicio, MinutoInicio);
  SepararHora(FormatDateTime('hh:nn',Saida)  , HoraFim, MinutoFim);

  Data := Entrada;
  while (Data < Saida) do
  begin
    Hora := StrToInt(FormatDateTime('hh', Data));
    if (Hora = HoraInicio) and (MinutoInicio <> 0) then
    begin
      Output.NewRecord;
      Output.Values['Horario'] := FormatDateTime('hh:nn',Entrada);
      Output.Values['Tempo'] := MinutoInicio/60;
      Data := IncMinute(Data, MinutoInicio);
    end else
    if (Hora = HoraFim) and (MinutoFim <> 0) then
    begin
        Output.NewRecord;
        Output.Values['Horario'] := FormatDateTime('hh:nn',Saida);
        Output.Values['Tempo'] := MinutoFim/60;
        Data :=  Saida;
    end else
    begin
      Output.NewRecord;
      Output.Values['Horario'] := FormatDateTime('hh:nn',Data);
      Output.Values['Tempo'] := 1;
      Data := IncHour(Data, 1);
    end;
  end;
end;


initialization
  wtsRegisterProc('APONTAMENTOS.Detalhar', Detalhar);

end.
