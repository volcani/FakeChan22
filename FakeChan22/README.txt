■これは何？

偽装ちゃん( https://github.com/k896951/FakeChan ) の後継となる、
偽装ちゃん22 ( https://github.com/k896951/FakeChan22 )です。

棒読みちゃんの替わりに起動すると、コメントビュアなどからのメッセージを受け取ってAssistantSeika経由で音声合成製品に発声させます。

AssistantSeikaはこちらからダウンロードしてください。
　 https://hgotoh.jp/wiki/doku.php/documents/voiceroid/assistantseika/assistantseika-001a


■変更点等( Ver 1.0.8 )

- プラグイン利用のための基本処理を実装。

- プラグイン処理実証の為、Twitterリスナを追加。
  ※このリスナ固有の設定箇所は先の実装処理により動的生成されています
  
  旧版を使っている人は旧版上で [全リスナ設定初期化] ボタンを押してから本版を起動してください。
  ※リスナの話者リストと置換リストの紐付けが初期化されているので再設定をお願いします。


■使用しているサードパーティ製品

 - Twitterアクセスの為に CoreTweet を利用しています。
   バージョン 1.0.0.483
   作成者     CoreTweet Development Team
   ライセンス https://opensource.org/licenses/mit-license.php

 - CoreTweetの依存関係により、Newtonsoft.Json を利用しています。
   バージョン 13.0.1
   作成者     James Newton-King
   ライセンス https://licenses.nuget.org/MIT

