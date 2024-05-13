import React, { useState } from "react";
import "./Hero.scss";
import Button from "../../../shared/components/Button/Button";
import TwitterIcon from "@mui/icons-material/Twitter";
import FacebookIcon from "@mui/icons-material/Facebook";
import InstagramIcon from "@mui/icons-material/Instagram";
import { Link } from "react-router-dom";
import ProfileImg from "../../../shared/utils/CloudImg";

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
          <Link to="/login" className="join-button">
            <Button>Join the Adventure</Button>
          </Link>
        </div>
        <div id="hero-social-links">
          <TwitterIcon className="hero-icons" />
          <FacebookIcon className="hero-icons" />
          <InstagramIcon className="hero-icons" />
        </div>
      </div>
    </section>
  );
};

export default Hero;
