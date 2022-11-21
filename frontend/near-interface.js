/* Talking with a contract often involves transforming data, we recommend you to encapsulate that logic into a class */

export class HelloNEAR {
  constructor({ contractId, walletToUse }) {
    this.contractId = contractId;
    this.wallet = walletToUse;    
  }

  async getNfts() {
    return await this.wallet.viewMethod({ contractId: 'nftv2.relik.testnet',  method: 'nft_tokens_for_owner', args: { account_id: 'pocket.testnet' }})
  }

}