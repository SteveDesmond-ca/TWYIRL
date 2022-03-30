# TWY IRL bot

Tweeting The Wonder Years lyrics referencing real life things

[![Run](https://github.com/SteveDesmond-ca/TWYIRL/actions/workflows/Run.yml/badge.svg)](https://github.com/SteveDesmond-ca/TWYIRL/actions/workflows/Run.yml)
[![CI](https://github.com/SteveDesmond-ca/TWYIRL/actions/workflows/CI.yml/badge.svg)](https://github.com/SteveDesmond-ca/TWYIRL/actions/workflows/CI.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=SteveDesmond-ca_TWYIRL&metric=coverage)](https://sonarcloud.io/summary/new_code?id=SteveDesmond-ca_TWYIRL)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=SteveDesmond-ca_TWYIRL&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=SteveDesmond-ca_TWYIRL)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=SteveDesmond-ca_TWYIRL&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=SteveDesmond-ca_TWYIRL)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=SteveDesmond-ca_TWYIRL&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=SteveDesmond-ca_TWYIRL)

Follow [@TWYIRL on Twitter](https://twitter.com/TWYIRL) for the tweets.

## Contribute Lyrics

If you'd like to contribute lyrics to this bot's repertoire, please first check [the list](TWYIRL/lyrics.md?plain=1), then DM the bot or create an issue [here](https://github.com/SteveDesmond-ca/TWYIRL/issues/new?template=new-lyrics.md).

## Requirements

- .NET SDK 6
- Twitter API keys set to the following environment variable names:
    - `ConsumerKey`
    - `ConsumerSecret`
    - `AccessKey`
    - `AccessSecret`

## Build from source

```
dotnet restore
dotnet build
dotnet test
```
