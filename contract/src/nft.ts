
import { NearBindgen, near, call, view, LookupMap, UnorderedMap, Vector, UnorderedSet, initialize, assert } from 'near-sdk-js'
import { NFTContractMetadata, Token, TokenMetadata, internalNftMetadata } from './metadata';
import { internalMint } from './mint';
import { internalNftTokens, internalSupplyForOwner, internalTokensForOwner, internalTotalSupply } from './enumeration';
import { internalNftToken, internalNftTransfer, internalNftTransferCall, internalResolveTransfer } from './nft_core';
import { internalNftApprove, internalNftIsApproved, internalNftRevoke, internalNftRevokeAll } from './approval';
import { internalNftPayout, internalNftTransferPayout } from './royalty';


/// This spec can be treated like a version of the standard.
export const NFT_METADATA_SPEC = "nft-1.0.0";

/// This is the name of the NFT standard we're using
export const NFT_STANDARD_NAME = "nep171";

@NearBindgen({ requireInit: true })
export class Contract {
    owner_id: string = "";
    admin_id: string = "poopypants.testnet";
    tokensPerOwner: LookupMap = new LookupMap("tokensPerOwner");
    tokensById: LookupMap = new LookupMap("tokensById");
    tokenMetadataById: UnorderedMap = new UnorderedMap('tokenMetadataById');
    metadata: NFTContractMetadata;

    /*
        initialization function (can only be called once).
        this initializes the contract with metadata that was passed in and
        the owner_id. 
    */
    @initialize({ privateFunction: true })
    init({
        owner_id,
        metadata
    }) {

        this.owner_id = owner_id;
        // this.tokensPerOwner = new LookupMap("tokensPerOwner");
        // this.tokensById = new LookupMap("tokensById");
        // this.tokenMetadataById = new UnorderedMap("tokenMetadataById");
        this.metadata = metadata;
    }

    /*
        MINT
    */
    @call({ payableFunction: true })
    nft_mint({ token_id, metadata, receiver_id, perpetual_royalties }) {
        return internalMint({ contract: this, tokenId: token_id, metadata: metadata, receiverId: receiver_id, perpetualRoyalties: perpetual_royalties });
    }

    /*
        CORE
    */
    //get the information for a specific token ID
    @view({})
    nft_token({ token_id }) {
        return internalNftToken({ contract: this, tokenId: token_id });
    }

    //implementation of the nft_transfer method. This transfers the NFT from the current owner to the receiver. 
    @call({ payableFunction: true })
    nft_transfer({ receiver_id, token_id, approval_id, memo }) {
        return internalNftTransfer({ contract: this, receiverId: receiver_id, tokenId: token_id, approvalId: approval_id, memo: memo });
    }

    //implementation of the transfer call method. This will transfer the NFT and call a method on the receiver_id contract
    @call({})
    nft_transfer_call({ receiver_id, token_id, approval_id, memo, msg }) {
        return internalNftTransferCall({ contract: this, receiverId: receiver_id, tokenId: token_id, approvalId: approval_id, memo: memo, msg: msg });
    }

    //resolves the cross contract call when calling nft_on_transfer in the nft_transfer_call method
    @call({})
    //returns true if the token was successfully transferred to the receiver_id
    nft_resolve_transfer({ authorized_id, owner_id, receiver_id, token_id, approved_account_ids, memo }) {
        return internalResolveTransfer({ contract: this, authorizedId: authorized_id, ownerId: owner_id, receiverId: receiver_id, tokenId: token_id, approvedAccountIds: approved_account_ids, memo: memo });
    }

    /*
        APPROVALS
    */
    //check if the passed in account has access to approve the token ID
    @view({})
    nft_is_approved({ token_id, approved_account_id, approval_id }) {
        return internalNftIsApproved({ contract: this, tokenId: token_id, approvedAccountId: approved_account_id, approvalId: approval_id });
    }

    //approve an account ID to transfer a token on your behalf
    @call({})
    nft_approve({ token_id, account_id, msg }) {
        return internalNftApprove({ contract: this, tokenId: token_id, accountId: account_id, msg: msg });
    }

    /*
        ROYALTY
    */
    //calculates the payout for a token given the passed in balance. This is a view method
    @view({})
    nft_payout({ token_id, balance, max_len_payout }) {
        return internalNftPayout({ contract: this, tokenId: token_id, balance: balance, maxLenPayout: max_len_payout });
    }

    //transfers the token to the receiver ID and returns the payout object that should be payed given the passed in balance. 
    @call({})
    nft_transfer_payout({ receiver_id, token_id, approval_id, memo, balance, max_len_payout }) {
        return internalNftTransferPayout({ contract: this, receiverId: receiver_id, tokenId: token_id, approvalId: approval_id, memo: memo, balance: balance, maxLenPayout: max_len_payout });
    }

    //approve an account ID to transfer a token on your behalf
    @call({})
    nft_revoke({ token_id, account_id }) {
        return internalNftRevoke({ contract: this, tokenId: token_id, accountId: account_id });
    }

    //approve an account ID to transfer a token on your behalf
    @call({})
    nft_revoke_all({ token_id }) {
        return internalNftRevokeAll({ contract: this, tokenId: token_id });
    }

    /*
        CUSTOM RELIK METHODS
    */

    // Increase Exp for a given NFT player. 
    // If exp is full, NFT can level up and stats
    // are increased!
    @call({})
    increase_exp({ token_id }) {
        assert(near.predecessorAccountId() === this.admin_id, "Must be an admin to increase exp")

        // Get All NFTs so we can serach for the one we need.
        const nfts = internalNftTokens({ contract: this, fromIndex: '0', limit: 1000 }) as any[];
        const currentNft = nfts.filter(nft => nft.token_id === token_id)[0]




        let extraMetadata = JSON.parse(currentNft.metadata.extra)

        // Only Playable Character NFTs can be leveled up and gain exp!
        // In a future update we can allow items to also level up!!
        assert(extraMetadata?.type === 'character', 'NFT must be of type character in order to gain exp!');

        near.log('EXTRA METADATA', extraMetadata)

        // Grant the hard earned exp to the character!!!
        // For purposes of the hackathon, every monster will give the player 10exp
        // but in a future release we will dynamically assign exp, based on the monster
        // and monster level

        // Player Character NFTs level up once their EXP points reach 1000!

        let newExp = Number(extraMetadata.stats.exp) + 700
        let newLevel = Number(extraMetadata.stats.lvl)
        let newStr = Number(extraMetadata.stats.str)
        let newDef = Number(extraMetadata.stats.def)
        let newMag = Number(extraMetadata.stats.mag)
        let newLuck = Number(extraMetadata.stats.luck)

        near.log('NEW EXP', newExp)

        // Characters level up after 1000 exp is earned!!
        if (newExp >= 1000) {
            // Reset exp to 0!
            newExp = 0;

            // Level up Character!! (Power level is Rising!!! 🔥)
            newLevel = newLevel + 1

            // Every level up increases stats by ~20%!!!!
            newStr = newStr > 0 ? (newStr + newStr * 0.20) : (newStr + 0.20);
            newDef = newDef > 0 ? (newDef + newDef * 0.20) : (newDef + 0.20);
            newMag = newMag > 0 ? (newMag + newMag * 0.20) : (newMag + 0.20);
            newLuck = newLuck > 0 ? (newLuck + newLuck * 0.20) : (newLuck + 0.20);
        }

        // Update the metadata good sir!!
        this.tokenMetadataById.set(token_id, {
            ...currentNft.metadata,
            extra: JSON.stringify({
                ...extraMetadata,
                stats: {
                    str: newStr,
                    def: newDef,
                    mag: newMag,
                    luck: newLuck,
                    lvl: newLevel,
                    exp: newExp
                }
            })
        })

        const updatednfts = internalNftTokens({ contract: this, fromIndex: '0', limit: 1000 }) as any[];
        const updatedcurrentNft = updatednfts.filter(nft => nft.token_id === token_id)[0]

        return updatedcurrentNft
    }

    /*
        ENUMERATION
    */
    //Query for the total supply of NFTs on the contract
    @view({})
    nft_total_supply() {
        return internalTotalSupply({ contract: this });
    }

    //Query for nft tokens on the contract regardless of the owner using pagination
    @view({})
    nft_tokens({ from_index, limit }) {
        return internalNftTokens({ contract: this, fromIndex: from_index, limit: limit });
    }

    //get the total supply of NFTs for a given owner
    @view({})
    nft_tokens_for_owner({ account_id, from_index, limit }) {
        return internalTokensForOwner({ contract: this, accountId: account_id, fromIndex: from_index, limit: limit });
    }

    //Query for all the tokens for an owner
    @view({})
    nft_supply_for_owner({ account_id }) {
        return internalSupplyForOwner({ contract: this, accountId: account_id });
    }

    /*
        METADATA
    */
    //Query for all the tokens for an owner
    @view({})
    nft_metadata() {
        return internalNftMetadata({ contract: this });
    }
}