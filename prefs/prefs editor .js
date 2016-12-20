//  
//  アプリケーション操作
//  
//  　同期でアプリケーションを起動する。
//  

//  ウインドウスタイル
var WS_NOTVISIVLE   = 0;    //ウインドウは非表示
var WS_ACT_NOMAL    = 1;    //ウインドウはアクティブ、サイズは通常(規定値)
var WS_ACT_MIN      = 2;    //ウインドウはアクティブ、サイズは最小
var WS_ACT_MAX      = 3;    //ウインドウはアクティブ、サイズは最大
var WS_NOTACT_NOMAL = 4;    //ウインドウは非アクティブ、サイズは通常
var WS_ACT_DEF      = 5;    //ウインドウはアクティブ、サイズは前回終了時と同じ
                            //(アプリによって動作は異なる)
var WS_NOTACT_MIN   = 7;    //ウインドウは非アクティブ、サイズは最小

//  Shell関連の操作を提供するオブジェクトを取得
var sh = new ActiveXObject( "WScript.Shell" );

//  Excelを起動する
sh.Run( "\"C:/Program Files/Microsoft Office/OFFICE11/EXCEL.EXE\"", WS_ACT_MAX, true );

//  Excelを終了したら表示
WScript.Echo("Excelは終了しました。");

//  オブジェクトを解放
sh = null;
