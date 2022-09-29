■これは何？

偽装ちゃん( https://github.com/k896951/FakeChan ) の後継となる、
偽装ちゃん22 ( https://github.com/k896951/FakeChan22 )です。

棒読みちゃんの替わりに起動すると、コメントビュアなどからのメッセージを受け取ってAssistantSeika経由で音声合成製品に発声させます。

AssistantSeikaはこちらからダウンロードしてください。
　 https://hgotoh.jp/wiki/doku.php/documents/voiceroid/assistantseika/assistantseika-001a


■変更点等( Ver 1.0.11 → 1.0.12 )

- DLLで拡張リスナを追加できるようにした。


■使用しているサードパーティ製品

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

     Microsoft.NETCore.Platforms    6.0.5
     Microsoft.Win32.Primitives    4.3.0
     NETStandard.Library    2.0.3
     System.AppContext    4.3.0
     System.Buffers    4.5.1
     System.Collections    4.3.0
     System.Collections.Concurrent    4.3.0
     System.Console    4.3.1
     System.Diagnostics.Debug    4.3.0
     System.Diagnostics.DiagnosticSource    6.0.0
     System.Diagnostics.Tools    4.3.0
     System.Diagnostics.Tracing    4.3.0
     System.Globalization    4.3.0
     System.Globalization.Calendars    4.3.0
     System.IO    4.3.0
     System.IO.Compression    4.3.0
     System.IO.Compression.ZipFile    4.3.0
     System.IO.FileSystem    4.3.0
     System.IO.FileSystem.Primitives    4.3.0
     System.Linq    4.3.0
     System.Linq.Expressions    4.3.0
     System.Memory    4.5.5
     System.Net.Http    4.3.4
     System.Net.Primitives    4.3.1
     System.Net.Sockets    4.3.0
     System.Numerics.Vectors    4.5.0
     System.ObjectModel    4.3.0
     System.Reflection    4.3.0
     System.Reflection.Extensions    4.3.0
     System.Reflection.Primitives    4.3.0
     System.Resources.ResourceManager    4.3.0
     System.Runtime    4.3.1
     System.Runtime.CompilerServices.Unsafe    6.0.0
     System.Runtime.Extensions    4.3.1
     System.Runtime.Handles    4.3.0
     System.Runtime.InteropServices    4.3.0
     System.Runtime.InteropServices.RuntimeInformation    4.3.0
     System.Runtime.Numerics    4.3.0
     System.Security.Cryptography.Algorithms    4.3.1
     System.Security.Cryptography.Encoding    4.3.0
     System.Security.Cryptography.Primitives    4.3.0
     System.Security.Cryptography.X509Certificates    4.3.2
     System.Text.Encoding    4.3.0
     System.Text.Encoding.Extensions    4.3.0
     System.Text.RegularExpressions    4.3.1
     System.Threading    4.3.0
     System.Threading.Tasks    4.3.0
     System.Threading.Timer    4.3.0
     System.Xml.ReaderWriter    4.3.1
     System.Xml.XDocument    4.3.0
