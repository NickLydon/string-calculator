# String calculator kata

Implementation of the kata taken from [Roy Osherove](https://osherove.com/tdd-kata-1).

I worked my way through the exercises and then realised the end result looked pretty ugly. It's impossible to glean from the implementation what the format of the input string looks like.

The only difficult parts are parsing, so I knew it could be improved via a parser combinator. I made a second attempt using the [sprache](https://github.com/sprache/Sprache) library, which I have prior experience with. It was more difficult to implement, due to:

1. Taking time to remember/look up how to do certain things with the library
2. Having a complete test suite tempted me to implement all the functionality at once. It would have been better to get the tests to pass one-by-one, rather than having some failing and not knowing why, or when they had failed

Although it was more effort to implement, it's my opinion that the implementation looks much clearer. Splitting the parsing into discrete chunks of functionality also means they could be tested separately if desired.

Afterwards I wondered if someone and beat me to it, and naturally [they had](https://davesquared.net/2013/07/string-calc-parsec.html) (albeit in Haskell).
