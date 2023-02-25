// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

abstract contract Test {
    function register() public virtual;

    function voteFor(address) public virtual;

    function numberOfVotesReceivedFor(address)
        public
        view
        virtual
        returns (uint256);
}

contract Voting is Test {
    struct UserData {
        bool isRegistered;
        uint256 votesReceived;
    }

    mapping(address => UserData) private registeredUsers;
    mapping(address => address) private votes;

    // adds the caller to the list of registered users
    function register() public override {
        require(
            !registeredUsers[msg.sender].isRegistered,
            "You are already registered"
        );
        registeredUsers[msg.sender].isRegistered = true;
    }

    // allows a registered user to cast a vote for another registered user
    function voteFor(address candidate) public override {
        require(msg.sender != candidate, "You can't vote for yourself...");
        require(
            registeredUsers[msg.sender].isRegistered,
            "Your are not registered. Only registered users can cast votes."
        );
        require(
            registeredUsers[candidate].isRegistered,
            "The candidate is not registered. Only registered users can recieve votes."
        );
        require(votes[msg.sender] == address(0), "You have already voted.");

        votes[msg.sender] = candidate;
        registeredUsers[candidate].votesReceived++;
    }

    // returns the number of votes received by a given address
    function numberOfVotesReceivedFor(address candidate)
        public
        view
        override
        returns (uint256)
    {
        require(
            registeredUsers[candidate].isRegistered,
            "The candidate is not registered. Only registered users can recieve votes."
        );

        return registeredUsers[candidate].votesReceived;
    }
}
