import { useState } from "react";
import { Link } from "react-router-dom";
import { navigationLinks } from "./navLinks";
import LoginIcon from "@mui/icons-material/Login";
import "./Header.scss";
import SideMenu from "./SideMenu/SideMenu";

const HeaderComponent = () => {
  const [isHovered, setIsHovered] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const [isSideMenuVisible, setIsSideMenuVisible] = useState(false);

  const onHover = (hovering: boolean): void => {
    setIsHovered(hovering);
  };

  const toggleSideMenu = () => {
    setIsSideMenuVisible(!isSideMenuVisible);
    isOpen ? setIsOpen(false) : setIsOpen(true);
  };

  return (
    <>
      <header id="header-main-container">
        <div id="header-container">
          <img
            src="/images/general/globe-header-logo.jpg"
            alt="AstroFit Logo"
            id="header-logo"
          />

          <nav id="header-navigation-container">
            <ul id="header-navigation">
              {navigationLinks.map((link) => (
                <li key={link.label}>
                  <Link to={link.path} className="navigation-links">
                    {link.label}
                  </Link>
                </li>
              ))}
            </ul>
          </nav>
          <div id="header-right">
            <Link
              to={"/account/login"}
              id="login-link-container"
              onMouseOver={() => onHover(true)}
              onMouseOut={() => onHover(false)}
            >
              <div id="login-link-content-container">
                <div
                  className={`login-link-icon-container ${
                    isHovered ? "login-link-icon-hover" : ""
                  }`}
                >
                  <LoginIcon className="login-icon" />
                </div>
                <div
                  className={`login-link-text ${
                    isHovered ? "login-text-hover" : ""
                  }`}
                >
                  Login
                </div>
              </div>
            </Link>
          </div>
        </div>
      </header>
      <button
        id="menu-button"
        title="side-menu-toggle"
        type="button"
        onClick={toggleSideMenu}
      >
        <div
          className={isOpen ? "open header-icons" : "header-icons"}
          id="menu-icon"
        >
          <span></span>
          <span></span>
          <span></span>
          <span></span>
        </div>
      </button>
      <nav id="side-menu-navigation" className={isOpen ? "active" : ""}>
        <Link
          to="/"
          onClick={toggleSideMenu}
          className="navigation-links-side-menu"
        >
          Home
        </Link>
        <Link
          to="/feed"
          className="navigation-links-side-menu"
          onClick={toggleSideMenu}
        >
          Feed
        </Link>
        <Link
          to="/landmarks"
          className="navigation-links-side-menu"
          onClick={toggleSideMenu}
        >
          Landmarks
        </Link>
      </nav>
    </>
  );
};

export default HeaderComponent;
