# efcore-net451-netcore50

Entity Framework Core (SQLite) を、 .NET Framework 4.5.1 と .NET Core それぞれのコンソールアプリで、 DI っぽく使うための Visual Studio 2015 プロジェクトサンプルです。

`./efcore-console-netcore/` ディレクトリ内の、 `appsettings.json`, `appsettings.Development.json` を変更することで、 SQLite のコネクション文字列や、 ログ表示の設定を、 DI的に行うことが可能です。

## efcore-console-netcore

.NET Core 用プロジェクトです。

`dotnet ef` コマンドや、 パッケージマネージャーコンソールで、 マイグレーションが行えます。

## efcore-console-net4

.NET Framework 4.5.1 用プロジェクトです。

パッケージマネージャーコンソールで、 マイグレーションが行えます。

プロジェクト内のソースコードは efcore-console-netcore のものをリンクして共有しています。

`appsettings.json`, `appsettings.Development.json` は、出力ディレクトリにコピーする設定にしていないと、 EF Tools 使用時や、アプリケーション実行時にエラーとなってしまいます。


## License

CC-0
