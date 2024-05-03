import { Link } from "react-router-dom";
import { navigationLinks } from "../navLinks";
import CloseIcon from "@mui/icons-material/Close";
import "./SideMenu.scss";

const SideMenu = ({
  isActive,
  toggleMenu,
}: {
  isActive: boolean;
  toggleMenu: () => void;
}) => {
  return (
    <nav id="side-menu-navigation" className={isActive ? "active" : ""}>
      {navigationLinks.map((link) => (
        <div key={link.label} onClick={toggleMenu}>
          <Link to={link.path} className="navigation-links-side-menu">
            {link.label}
          </Link>
        </div>
      ))}
      <button
        type="button"
        title="Close"
        id="side-menu-button"
        onClick={toggleMenu}
      >
        <CloseIcon id="close-icon" />
      </button>
    </nav>
  );
};

export default SideMenu;
