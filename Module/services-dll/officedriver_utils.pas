unit officedriver_utils;

interface

procedure LogDebug(msg: string; prefix: string = 'Calculo');
procedure Log(msg: string; prefix: string = 'Calculo');
function TimeToMinute(Date: TDateTime): Integer;
function DecimalToHour(Value: Real): string;
function HourToDecimal(Value: string): Real;

implementation

uses
  LogFiles, Windows, SysUtils;

procedure Log(msg: string; prefix: string = 'Calculo');
begin
  AddLog(0,msg, 'Calculo');
end;

procedure LogDebug(msg: string; prefix: string = 'Calculo');
begin
  {$IFDEF MSWINDOWS}
  if Length(msg) > 0 then OutputDebugString(PChar('Calculo:> ' + msg));
  Log(msg);
  {$ENDIF}
end;

function TimeToMinute(Date: TDateTime): Integer;
var Hours, Minutes, Seconds, MSeconds : word;
begin
  DecodeTime(date, Hours, Minutes, Seconds, MSeconds);
  result := Minutes + (Hours * 60);
end;

function HourToDecimal(Value: string): Real;
begin
  Result := StrToIntDef(Copy(Value, 1,2),0) + (StrToIntDef(Copy(Value, 4,2),0) / 60);
end;

function DecimalToHour(Value: Real): string;
var
  AuxM, AuxH : Real;
  Hour, Minute: String;
begin
  AuxH := Trunc(Value);
  Hour := FormatFloat('00',AuxH);
  AuxM := (Value - AuxH) * 60;
  Minute := FormatFloat('00',AuxM);
  if Round(AuxM) = 60 then
  begin
    Hour := FormatFloat('00',AuxH+1);
    Minute := '00';
  end;

  Result := Hour + ':' + Minute;
end;


end.
