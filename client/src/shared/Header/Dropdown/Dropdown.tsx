// DropdownMenu.tsx
import { useState, useEffect } from "react";
import Avatar from "../../components/Avatar/Avatar";
import "./Dropdown.scss";
import { Link, useNavigate } from "react-router-dom";
import { Logout, NavigateNext, Person, Shield } from "@mui/icons-material";
import { useDispatch } from "react-redux";
import { setLogout } from "../../../state/features/authSlice";

const Dropdown = () => {
  const navigate = useNavigate();
  const [isOpen, setIsOpen] = useState(false);
  const toggleDropdown = () => setIsOpen(!isOpen);
  const dispatch = useDispatch();
  const user = localStorage.getItem("user");
  if (!user) {
    navigate("/account/login");
  }
  const userParsed = JSON.parse(user!);
  const userId = userParsed.id;
  const isGuide = userParsed.roles.includes("Guide");

  const handleLogout = () => {
    dispatch(setLogout());
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  };

  useEffect(() => {
    const handleOutsideClick = (event: any) => {
      if (isOpen && !event.target.closest("#dropdown-icon-button")) {
        setIsOpen(false);
      }
    };

    if (isOpen) {
      window.addEventListener("click", handleOutsideClick);
    }

    return () => {
      window.removeEventListener("click", handleOutsideClick);
    };
  }, [isOpen]);

  return (
    <button
      onClick={toggleDropdown}
      title="dropdown-button"
      type="button"
      id="dropdown-icon-button"
    >
      <Avatar photoUrl={null} />
      <div
        className={`dropdown-container ${isOpen ? "dropdown-open" : ""}`}
        id="dropdown-container"
      >
        <Link to={"/profile/" + userId} className="dropdown-item">
          <Person className="dropdown-icon" />
          <p>Profile</p>
          <NavigateNext className="dropdown-icon" />
        </Link>
        {isGuide && (
          <Link to={"/users"} className="dropdown-item users">
            <Shield className="dropdown-icon" />
            <p>Users</p>
            <NavigateNext className="dropdown-icon" />
          </Link>
        )}
        <div className="dropdown-item logout" onClick={handleLogout}>
          <Logout className="dropdown-icon" />
          <p>Logout</p>
          <NavigateNext className="dropdown-icon" />
        </div>
      </div>
    </button>
  );
};

export default Dropdown;
