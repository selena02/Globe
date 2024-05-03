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
        />

        <FeatureCard
          iconName="place"
          title="Landmark Detector"
          description="Discover and learn about landmarks as you visit them. Our landmark detector provides historical insights and saves each discovery to your personal travel log."
        />

        <FeatureCard
          iconName="emoji_events"
          title="Achievements"
          description="Engage with the community and our features to earn achievements. Each interaction brings new rewards, enhancing your travel experience and bragging rights."
        />

        <FeatureCard
          iconName="account_circle"
          title="Profiles"
          description="Explore profiles of fellow travelers, connect with like-minded adventurers, and expand your network. Your next journey companion might just be a profile away."
        />
      </div>
    </div>
  </section>
);

export default About;
