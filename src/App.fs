module App

// const tmi = require('tmi.js');

// const client = new tmi.Client({
//   connection: {
//     secure: true,
//     reconnect: true
//   },
//   channels: [ 'my-name' ]
// });

// client.connect();

// client.on('message', (channel, tags, message, self) => {
//   // "Alca: Hello, World!"
//   console.log(`${tags['display-name']}: ${message}`);
// });

open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open System
open System.Collections.Generic

type IConnectionOpts =
  abstract secure : bool with get, set
  abstract reconnect : bool with get, set

type IClientOpts =
  abstract connection : IConnectionOpts with get, set
  abstract channels : ResizeArray<string> with get, set

type Handler = Func<string, obj, string, obj, Unit>

type IClient =
  abstract connect : Unit -> Unit
  abstract on : string * Handler -> Unit

type ITmiModule =
  abstract Client : IClientOpts -> IClient

[<Import("default", from="tmi.js")>]
let tmiModule : ITmiModule = jsNative

let opts : IClientOpts =
  !!createObj
    [
      "connection" ==>
        createObj
          [ "secure" ==> true
            "reconnect" ==> true ]
      "channels" ==> List<_> [ "summit1g" ]
    ]

let client = tmiModule.Client opts

client.connect ()

client.on
  (
    "message",
    new Func<_, _, _, _, _> (fun channel tags message self ->
      console.log (channel, tags, message, self))
  )
