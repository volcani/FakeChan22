# 偽装ちゃん22(FakeChan22) - 偽装ちゃん(FakeChan)の後継

機能は大体同じですがもう少し細かい設定が出来るようにします。AssistantSeikaが必須なのは変わりません。

![image](https://user-images.githubusercontent.com/22530106/192142882-3d7ae2e4-ba15-46a1-a407-dfd9d05569d2.png)

## まだ制作中です

不具合はまだまだあるはずなので、Issuesにてご連絡いただければと。

## 前の版と違う機能

 - AssistantSeikaが認識していない製品の話者を指定された場合、単純に無視するようになりました。エラーメッセージ等も出しません。
 - 初期設定ではコメント読み上げしません。受信口（リスナ）を明示的に有効にする必要があります。
 - 話者リストを最低1つ作成する必要があります。話者(cid)を必要とする機能は全て話者リストを参照するようになりました。
 - APIの NOWPLAYING はキューの残量がゼロで発声中（同期発声中）ではない時にFalseを返します。
 - Twitterを検索してしゃべる機能を追加しました。
 - 偽装ちゃん(FakeChan)ではjsonで記述していた呟き定義をGUIで実施可能になりました。
 - 絵文字削除の機能を取り込みました。置換リストの定義で指定可能です。
 - w(わら),888(パチパチパチ) の置換は組み込みになりました。定義不要です。
 - 非日本語判定は現時点でもいい加減です。中国語を日本語と判定したりしてしまいます。
 - リスナDLLを追加して拡張できるようにしました。サンプルは[偽装ちゃん22用拡張リスナのサンプル](https://github.com/k896951/TaskSampleTask)にあります。

他にも書き忘れがあるかもしれません。

## その他

### もう少し詳しい内容

もう少し詳しい内容は https://hgotoh.jp/wiki/doku.php/documents/tools/tools-207 を参照してください。  

### ダウンロード

ダウンロードは[リリース](https://github.com/k896951/FakeChan22/releases)からお願いします。

### 使用しているサードパーティ製品

 - Twitterアクセスの為に CoreTweet を利用しています。  
   バージョン 1.0.0.483  
   作成者     CoreTweet Development Team  
   ライセンス https://opensource.org/licenses/mit-license.php  

 - CoreTweetの依存関係により、Newtonsoft.Json を利用しています。  
   バージョン 13.0.1  
   作成者     James Newton-King  
   ライセンス https://licenses.nuget.org/MIT  

 - DLLをまとめる目的で Costura.Fody を利用しています。  
   バージョン 5.7.0  
   作成者     geertvanhorrik,simoncropp  
   ライセンス https://licenses.nuget.org/MIT  

 - Costura.Fody の依存関係により Fody を利用しています。  
   バージョン 6.6.3  
   作成者     Fody  
   ライセンス https://www.nuget.org/packages/Fody/6.6.3/license  

 - 依存関係によりMicrosoftのライブラリを利用しています。  
  
     ライセンスが  
       https://licenses.nuget.org/MIT  
       https://dotnet.microsoft.com/ja-jp/dotnet_library_license.htm  
  
     のものがあります。  
  
   -   Microsoft.NETCore.Platforms    6.0.5
   -   Microsoft.Win32.Primitives    4.3.0
   -   NETStandard.Library    2.0.3
   -   System.AppContext    4.3.0
   -   System.Buffers    4.5.1
   -   System.Collections    4.3.0
   -   System.Collections.Concurrent    4.3.0
   -   System.Console    4.3.1
   -   System.Diagnostics.Debug    4.3.0
   -   System.Diagnostics.DiagnosticSource    6.0.0
   -   System.Diagnostics.Tools    4.3.0
   -   System.Diagnostics.Tracing    4.3.0
   -   System.Globalization    4.3.0
   -   System.Globalization.Calendars    4.3.0
   -   System.IO    4.3.0
   -   System.IO.Compression    4.3.0
   -   System.IO.Compression.ZipFile    4.3.0
   -   System.IO.FileSystem    4.3.0
   -   System.IO.FileSystem.Primitives    4.3.0
   -   System.Linq    4.3.0
   -   System.Linq.Expressions    4.3.0
   -   System.Memory    4.5.5
   -   System.Net.Http    4.3.4
   -   System.Net.Primitives    4.3.1
   -   System.Net.Sockets    4.3.0
   -   System.Numerics.Vectors    4.5.0
   -   System.ObjectModel    4.3.0
   -   System.Reflection    4.3.0
   -   System.Reflection.Extensions    4.3.0
   -   System.Reflection.Primitives    4.3.0
   -   System.Resources.ResourceManager    4.3.0
   -   System.Runtime    4.3.1
   -   System.Runtime.CompilerServices.Unsafe    6.0.0
   -   System.Runtime.Extensions    4.3.1
   -   System.Runtime.Handles    4.3.0
   -   System.Runtime.InteropServices    4.3.0
   -   System.Runtime.InteropServices.RuntimeInformation    4.3.0
   -   System.Runtime.Numerics    4.3.0
   -   System.Security.Cryptography.Algorithms    4.3.1
   -   System.Security.Cryptography.Encoding    4.3.0
   -   System.Security.Cryptography.Primitives    4.3.0
   -   System.Security.Cryptography.X509Certificates    4.3.2
   -   System.Text.Encoding    4.3.0
   -   System.Text.Encoding.Extensions    4.3.0
   -   System.Text.RegularExpressions    4.3.1
   -   System.Threading    4.3.0
   -   System.Threading.Tasks    4.3.0
   -   System.Threading.Timer    4.3.0
   -   System.Xml.ReaderWriter    4.3.1
   -   System.Xml.XDocument    4.3.0
