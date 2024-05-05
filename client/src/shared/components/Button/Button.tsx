import React, { useState } from "react";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import "./Button.scss";

type ButtonProps = {
  children?: React.ReactNode;
};

const ButtonComponent: React.FC<ButtonProps> = ({ children }) => {
  const [isHovered, setIsHovered] = useState(false);

  const onButtonHover = (hover: boolean) => {
    setIsHovered(hover);
  };

  return (
    <button
      type="button"
      onMouseOver={() => onButtonHover(true)}
      onMouseOut={() => onButtonHover(false)}
      className="button"
    >
      <div className="button-content">
        {children}
        <ArrowForwardIcon className="button-content-icon" />{" "}
      </div>
    </button>
  );
};

export default ButtonComponent;
