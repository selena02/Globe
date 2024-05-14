import React, { useState, useRef, useEffect } from "react";
import SearchIcon from "@mui/icons-material/Search";
import InputBase from "@mui/material/InputBase";
import IconButton from "@mui/material/IconButton";
import "./SearchBar.scss";
import fetchAPI from "../../utils/fetchAPI";
import { handleApiErrors } from "../../utils/displayApiErrors";
import Avatar from "../../components/Avatar/Avatar";
import { Link } from "react-router-dom";

interface User {
  userId: number;
  username: string;
  fullName: string;
  profilePicture: string | null;
}
const SearchBar = () => {
  const [isExpanded, setIsExpanded] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);
  const [users, setUsers] = useState<User[]>([]);
  const [filteredUsers, setFilteredUsers] = useState<User[]>([]);
  const [searchInput, setSearchInput] = useState("");
  const debounceTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  useEffect(() => {
    fetchSearchUsers();

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

  const expandSearch = () => setIsExpanded(true);
  const collapseSearch = () => {
    setIsExpanded(false);
    setFilteredUsers([]);
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const query = event.target.value;
    setSearchInput(query);
    if (debounceTimeoutRef.current) {
      clearTimeout(debounceTimeoutRef.current);
    }
    debounceTimeoutRef.current = setTimeout(() => {
      const filtered = users.filter((user) =>
        user.username.toLowerCase().includes(query.toLowerCase())
      );
      setFilteredUsers(filtered);
    }, 200);
  };

  const fetchSearchUsers = async () => {
    try {
      const data = await fetchAPI<{ users: User[] }>("users/search-bar", {
        method: "GET",
      });
      setUsers(data.users);
    } catch (error) {
      handleApiErrors(error);
    }
  };

  return (
    <div
      ref={containerRef}
      className={`search-bar-container ${isExpanded ? "expanded" : ""}`}
    >
      <IconButton
        id="search-button"
        className={isExpanded ? "expanded" : ""}
        onClick={expandSearch}
      >
        <SearchIcon id="search-icon" />
      </IconButton>
      {isExpanded && (
        <InputBase
          placeholder="Search..."
          inputProps={{ "aria-label": "search" }}
          className="search-input"
          autoFocus
          value={searchInput}
          onChange={handleInputChange}
        />
      )}
      <div className={isExpanded ? "search-results" : "none"}>
        {filteredUsers.map((user) => (
          <Link
            to={`profile/${user.userId}`}
            key={user.userId}
            className="search-result-item"
          >
            <Avatar photoUrl={user.profilePicture} />
            <div className="names">
              <h2>{user.username}</h2>
              <p>{user.fullName}</p>
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
};

export default SearchBar;
