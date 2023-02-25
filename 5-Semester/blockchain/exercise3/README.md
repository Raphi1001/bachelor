# blockchain-exercise 3

Go to the online IDE https://remix.ethereum.org/, select "Injected Web3" and connect to your account selected in MetaMask.

Implement a voting smart contract that fulfills the following requirements:

    users must register before they can cast a vote
    registered users can only cast votes for other registered users
    registered users can only cast one vote and can not change it

The resulting smart contract should be derived from the following smart contract and implement the following specified functions:

abstract contract Test {
    function register() public virtual;
    function voteFor(address) public virtual;
    function numberOfVotesReceivedFor(address) view public virtual returns(uint);
}

Attach the .sol file containing the source code of the smart contract, deploy the smart contract on the Goerli network and enter its address! 
(0xDB04032A58e892efdFc935c290Ae8577E562405c)


Extend your smart contract to implement the following function too:

    function winnersAndNumberOfWinningVotes() view public returns(address[] memory, uint);

Attach the .sol file containing the source code of the smart contract, deploy the smart contract on the Goerli network and enter its address! 
(0x0d2845a0de1873AC41D0DAaa947Eb0f800BAdAAD)