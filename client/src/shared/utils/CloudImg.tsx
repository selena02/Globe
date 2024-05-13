import { Cloudinary } from "@cloudinary/url-gen";
import { AdvancedImage } from "@cloudinary/react";
import { thumbnail } from "@cloudinary/url-gen/actions/resize";

const cld = new Cloudinary({
  cloud: {
    cloudName: "dsdleukb7",
  },
});

const ProfileImg = ({ publicId }: { publicId: string }) => {
  const myImage = cld
    .image(publicId)
    .resize(thumbnail().width(80).height(80))
    .format("webp")
    .quality("auto");

  return (
    <AdvancedImage
      cldImg={myImage}
      style={{
        objectFit: "cover",
      }}
    />
  );
};
export default ProfileImg;
