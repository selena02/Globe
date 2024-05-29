import "./Hero.scss";
import Button from "../../../shared/components/Button/Button";
import { Link } from "react-router-dom";

const Hero = () => {
  return (
    <section id="landing-page-container">
      <div id="landing-page-content-container">
        <img id="plane" src="/images/home/hero-plane.png" alt="Plane" />
        <img id="cloud" src="/images/home/hero-cloud.png" alt="Cloud" />
        <div id="landing-page-content">
          <h1 id="explore-title">TRAVEL CONNECTED</h1>
          <h2 id="main-title">GLOBE</h2>{" "}
          <p id="landing-page-slogan">Connect, Share, Discover</p>
          <Link to="/account/login" className="join-button">
            <Button>Join the Adventure</Button>
          </Link>
        </div>
      </div>
    </section>
  );
};

export default Hero;
