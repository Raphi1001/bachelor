// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/utils/Counters.sol";

contract StrengthNFT is ERC721URIStorage, Ownable {
    using Counters for Counters.Counter;
    Counters.Counter private _idsTokens;

    constructor() ERC721("Strength NFTs", "SNFT") {}

    function totalSupply() public view returns (uint256) {
        return _idsTokens.current();
    }

    function awardItem(address character, string memory tokenURI)
        public
        onlyOwner
        returns (uint256)
    {
        uint256 newItemId = _idsTokens.current();
        _mint(character, newItemId);
        _setTokenURI(newItemId, tokenURI);

        _idsTokens.increment();
        return newItemId;
    }

    function mintItemSpecific(address minteradd, string memory token)
        public
        onlyOwner
        returns (uint256)
    {
        uint256 id = 1234;
        _mint(minteradd, id);
        _setTokenURI(id, token);
        return id;
    }
}
