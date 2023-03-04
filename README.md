# 偽装ちゃん22(FakeChan22)

機能は大体同じですがもう少し細かい設定が出来るようにします。AssistantSeikaが必須なのは変わりません。

![image](https://user-images.githubusercontent.com/22530106/192142882-3d7ae2e4-ba15-46a1-a407-dfd9d05569d2.png)

## まだ制作中です

不具合はまだまだあるはずなので、Issuesにてご連絡いただければと。

## 偽装ちゃん(FakeChan)には無かった機能

 - 話者リストを最低1つ作成する必要があります。話者(cid)を必要とする機能は全て話者リストを参照するようになりました。
 - メッセージ中の絵文字削除を行う機能を追加しました。置換リストの定義で指定可能です。
 - メッセージ中のw(わら),888(パチパチパチ) の置換は組み込みになりました。定義不要です。
 - 偽装ちゃん(FakeChan)ではjsonで記述していた呟き定義をGUIで実施可能になりました。
 - Twitterを検索してしゃべるリスナを追加しました。※オプショナル
 - いくつかの機能をオプショナルとし、DLLで追加するようにしました（1.0.17以降）
 - 自分で拡張リスナを追加できるようにしました。サンプルコードは[偽装ちゃん22用拡張リスナのサンプル](https://github.com/k896951/TaskSampleTask)にあります。

## 偽装ちゃん(FakeChan)とは動作が違う機能

 - AssistantSeikaが認識していない製品の話者を指定された場合、単純に無視するようになりました。エラーメッセージ等も出しません。
 - 初期設定ではコメント読み上げしません。受信口（リスナ）を明示的に有効にする必要があります。
 - APIの NOWPLAYING はキューの残量がゼロで発声中（同期発声中）ではない時にFalseを返します。

他にも書き忘れがあるかもしれません。

## その他

 - 非日本語判定は現時点でもいい加減です。中国語を日本語と判定したりしてしまいます。

### もう少し詳しい内容

 - もう少し詳しい内容は https://hgotoh.jp/wiki/doku.php/documents/tools/tools-207 を参照してください。  

### ダウンロード

 - ダウンロードは[リリース](https://github.com/k896951/FakeChan22/releases)からお願いします。

### 使用しているサードパーティ製品

 - assemblyをまとめる目的で Costura.Fody を利用しています。  
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
  
   -   Microsoft.NETCore.Platforms    7.0.0
   -   NETStandard.Library    2.0.3
   -   System.Buffers    4.5.1
   -   System.Memory    4.5.5
   -   System.Numerics.Vectors    4.5.0
   -   System.Runtime.CompilerServices.Unsafe    6.0.0
