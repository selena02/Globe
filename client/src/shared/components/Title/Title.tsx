import React from "react";
import "./Title.scss";

type TitleProps = {
  title: string;
};

const Title: React.FC<TitleProps> = ({ title }) => {
  return (
    <div className="title-container">
      <span className="title-text">{title}</span>
      <span className="title-text-small">{title}</span>
    </div>
  );
};

export default Title;
