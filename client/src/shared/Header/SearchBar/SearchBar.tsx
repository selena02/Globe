import React, { useState, useRef, useEffect } from "react";
import SearchIcon from "@mui/icons-material/Search";
import InputBase from "@mui/material/InputBase";
import IconButton from "@mui/material/IconButton";
import "./SearchBar.scss";

const SearchBar = () => {
  const [isExpanded, setIsExpanded] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);

  const expandSearch = () => setIsExpanded(true);
  const collapseSearch = () => setIsExpanded(false);

  // Handle clicking outside of the search bar
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        containerRef.current &&
        !containerRef.current.contains(event.target as Node)
      ) {
        collapseSearch();
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  return (
    <div
      ref={containerRef}
      className={`search-bar-container ${isExpanded ? "expanded" : ""}`}
    >
      <button
        id="search-button"
        className={isExpanded ? "expanded" : ""}
        title="search-button"
        onClick={expandSearch}
      >
        <SearchIcon id="search-icon" />
      </button>
      {isExpanded && (
        <InputBase
          placeholder="Search..."
          inputProps={{ "aria-label": "search" }}
          className="search-input"
          autoFocus
        />
      )}
    </div>
  );
};

export default SearchBar;
