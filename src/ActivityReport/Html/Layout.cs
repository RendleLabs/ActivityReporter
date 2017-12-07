namespace ActivityReport.Html
{
    internal static class Layout
    {
        public const string Css = @"
body { margin: 8px 0; }
hr { color: white; margin: 0; border-style: solid; }
.split { border-width: 0; min-height: 20px; }
.root { background-color: darkgray; margin-left: 240px; }
.split-label { font-family: monospace; font-size: 10pt; position: absolute; height: 20px; width: 240px; max-width: 240px; display: flex; background-color: lightgray; }
.split-label span { text-overflow: ellipsis; overflow: hidden; display: inline-block; white-space: nowrap; color: black; margin-left: 4px }
.level-0>.split-label { left: 8px; width: 232px; }
.level-1>.split-label { left: 16px; width: 224px; }
.level-2>.split-label { left: 24px; width: 216px; }
.level-3>.split-label { left: 32px; width: 208px; }
.level-4>.split-label { left: 40px; width: 200px; }
.level-5>.split-label { left: 48px; width: 192px; }
.level-0>.split-label>span { width: 200px; }
.level-1>.split-label>span { width: 192px; }
.level-2>.split-label>span { width: 184px; }
.level-3>.split-label>span { width: 176px; }
.level-4>.split-label>span { width: 168px; }
.level-5>.split-label>span { width: 160px; }
.split:hover>.split-label { font-weight: bold; background-color: darkgray; }
.spacer { height: 16px; font-family: sans-serif; font-size: 9pt; padding-top: 4px; padding-left: 4px; color: white; }
.collapsed>.sub { display: none; }
button.subs-0 { visibility: hidden; }
.toggle-button { width: 16px; height: 16px; border: solid 1px #333333; background-color: white; font-size: 8pt; border-radius: 8px; color: #333333; transform: rotate(90deg);  }
.collapsed .toggle-button { transform: rotate(0deg); }
";
    }
}