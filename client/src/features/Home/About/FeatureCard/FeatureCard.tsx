import React from "react";
import LanguageIcon from "@mui/icons-material/Language";
import PlaceIcon from "@mui/icons-material/Place";
import EmojiEventsIcon from "@mui/icons-material/EmojiEvents";
import Person from "@mui/icons-material/Person";
import EastIcon from "@mui/icons-material/East";
import "./FeatureCard.scss";

type FeatureCardProps = {
  iconName: string;
  title: string;
  description: string;
};

const iconMapper = {
  language: LanguageIcon,
  place: PlaceIcon,
  emoji_events: EmojiEventsIcon,
  account_circle: Person,
};

const FeatureCard: React.FC<FeatureCardProps> = ({
  iconName,
  title,
  description,
}) => {
  const [isHovered, setIsHovered] = React.useState(false);
  const Icon = iconMapper[iconName as keyof typeof iconMapper];

  return (
    <div
      className={`feature-card ${isHovered ? "highlighted" : ""}`}
      onMouseOver={() => setIsHovered(true)}
      onMouseOut={() => setIsHovered(false)}
    >
      <div>
        {Icon ? (
          <Icon className={`feature-icons ${isHovered ? "highlighted" : ""}`} />
        ) : null}
        <div className={`feature-title ${isHovered ? "highlighted" : ""}`}>
          {title}
        </div>
        <div className={`feature-text ${isHovered ? "highlighted" : ""}`}>
          {description}
        </div>
      </div>
      <EastIcon className={`arrow ${isHovered ? "highlighted" : ""}`} />
    </div>
  );
};

export default FeatureCard;
