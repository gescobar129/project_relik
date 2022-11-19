import { FungibleToken } from './ft'

export class FTContractMetadata {
	spec: string;
	name: string;
	symbol: string;
	decimals: number;

	icon?: string;
	base_uri?: string;
	reference?: string;
	reference_hash?: string;

	constructor({
		spec,
		name,
		symbol,
		decimals,
		icon,
		reference,
		reference_hash,
	}: {
		spec,
		name,
		symbol,
		decimals,
		icon,
		reference,
		reference_hash,
	}) {
		this.spec = spec
		this.name = name
		this.symbol = symbol
		this.icon = icon
		this.reference = reference
		this.reference_hash = reference_hash
		this.decimals = decimals
	}
}

export function internalFTMetadata({ contract }: { contract: FungibleToken }): FTContractMetadata {
	return contract.metadata
} 