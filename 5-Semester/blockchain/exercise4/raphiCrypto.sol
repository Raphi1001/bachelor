// contracts/raphiCrypto.sol
// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC20/ERC20.sol";

contract RaphiTokenPrivate is ERC20 {
    mapping(address => bool) private whitelisted;

    constructor() ERC20("RaphiTokenPrivate", "RTP") {
        uint256 initialSupply = 1000000000000000000000;

        _mint(msg.sender, initialSupply);

        whitelisted[msg.sender] = true;
    }

    function whitelist(address recipient) public {
        require(
            balanceOf(msg.sender) > 0,
            "Only token holders can whitelist recipients."
        );
        require(!whitelisted[recipient], "Recipient is already whitelisted.");

        whitelisted[recipient] = true;
    }

    function transfer(address to, uint256 amount)
        public
        virtual
        override
        returns (bool)
    {
        /*
        @dev "vulnerability" bc only the address of a token holder is currently needed. Token holder does not need to execute the function
        ... how do i find out who executes the function? According to debugger: msg.sender == to 
        */
        require(
            whitelisted[to],
            "Recipient must be whitelisted by a token holder to receive tokens."
        );

        address owner = _msgSender();
        _transfer(owner, to, amount);
        return true;
    }

    function transferFrom(
        address from,
        address to,
        uint256 amount
    ) public virtual override returns (bool) {
        require(
            whitelisted[to],
            "Recipient must be whitelisted by a token holder to receive tokens."
        );

        address spender = _msgSender();
        _spendAllowance(from, spender, amount);
        _transfer(from, to, amount);
        return true;
    }
}
