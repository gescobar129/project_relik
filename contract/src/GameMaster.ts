import { NearBindgen, call, view, initialize, near, bytes, assert } from "near-sdk-js";

/* 
GameMaster Contract:
	The GameMaster Contract owns all $GOLD tokens and all Loot NFTs.
	
	- owner: The address/account that deploys the contract. 
			 It's the only account that can add and remove managers.




*/


@NearBindgen({ requireInit: true })
export class GameMaster {
	owner: string;

	constructor() {
		this.owner = "";
	}

	@initialize({})
	init({ owner }: { owner: string }) {
		this.owner = owner;
	}

	@call({})
	set_new_owner({ new_owner }: { new_owner: string }) {
		assert(near.predecessorAccountId() === this.owner, "Must be called by GameMaster Owner")

		this.owner = new_owner
	}

	@call({})
	transfer_loot({ to }: { to: string }) {
		assert(near.predecessorAccountId() === this.owner, "Must be called by GameMaster Owner")

		// Transfer NFT logic here
	}

	@call({})
	transfer_gold({ to, amount }: { to: string, amount: BigInt }) {
		assert(near.predecessorAccountId() === this.owner, "Must be called by GameMaster Owner")

		// Transfer GOLD logic here
	}

	@view({})
	get_owner() {
		return this.owner
	}




}