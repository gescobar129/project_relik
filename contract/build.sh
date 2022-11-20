#!/bin/sh

echo ">> Building contract"

near-sdk-js build src/nft.ts build/nft.wasm
# near-sdk-js build src/GameMaster.ts build/GameMaster.wasm
# near-sdk-js build src/ft.ts build/ft.wasm
# near-sdk-js build src/GoldToken.ts build/GoldToken.wasm
