export interface NFT {
	token_id: 7;
	owner_id: "pocket.testnet";
	metadata: {
		title: string;
		description: string;
		media: string;
		media_hash: string;
		copies: number;
		extra: string;
	}
}

export interface GetAccountResponse {
	accountId: string;
	nearBalance: number;
	goldBalance: number;
	ownedNfts: {
		loot: NFT[];
		characters: NFT[];
	}
}
