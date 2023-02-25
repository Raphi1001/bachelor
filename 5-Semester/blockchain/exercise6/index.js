import { ethers } from "ethers";

async function main() {
    // Configuring the connection to an Ethereum node
    await window.ethereum.enable()
    const provider = new ethers.providers.Web3Provider(window.ethereum);

    const tx = await provider.getBlockNumber()
    // Waiting for the transaction to be mined
    // The transaction is now on chain!
    console.log(tx);
}

main();