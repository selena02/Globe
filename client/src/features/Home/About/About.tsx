import FeatureCard from "./FeatureCard/FeatureCard";
import "./About.scss";

const About = () => (
  <section id="about-section">
    <div id="about-container">
      <div id="feature-cards-container">
        <FeatureCard
          iconName="language"
          title="Community Feed"
          description="Dive into a vibrant community feed where travelers share their exciting journeys and tips. Connect, explore, and get inspired by fellow adventurers."
          route="/feed"
        />

        <FeatureCard
          iconName="place"
          title="Landmark Detector"
          description="Discover and learn about landmarks as you visit them. Our landmark detector provides historical insights and saves each discovery to your personal travel log."
          route="/landmark"
        />

        <FeatureCard
          iconName="map"
          title="Explore"
          description="Discover where other users have journeyed to on a world map and see the paths taken by fellow adventurers. Your next travel inspiration is just a map view away."
          route="/explore"
        />

        <FeatureCard
          iconName="account_circle"
          title="Profiles"
          description="Explore profiles of fellow travelers, connect with like-minded adventurers, and expand your network. Your next journey companion might just be a profile away."
          route="/account"
        />
      </div>
      <div id="about-us-content-container">
        <div id="about-text-container">
          <h2 id="about-title">About Us</h2>
          <p id="about-description">
            Globe is a platform that provides a one-stop solution for travelers
            to share their amazing experiences, connect with fellow travelers,
            and explore new destinations. Our mission is to inspire people to
            travel and enjoy life to the fullest!
          </p>
          <img
            id="about-image-2"
            src="/images/home/picture-frames-2.png"
            alt="Pictures of vacations 2"
          />
        </div>
        <img
          id="about-image"
          src="/images/home/picture-frames.png"
          alt="Pictures of vacations"
        />
      </div>
    </div>
  </section>
);

export default About;
