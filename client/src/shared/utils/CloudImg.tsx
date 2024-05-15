import { Cloudinary } from "@cloudinary/url-gen";
import { AdvancedImage } from "@cloudinary/react";
import { thumbnail } from "@cloudinary/url-gen/actions/resize";

const cld = new Cloudinary({
  cloud: {
    cloudName: "dx9pfv5oz",
  },
});

export const ProfileImg = ({ publicId }: { publicId: string }) => {
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

export const PostImg = ({ publicId }: { publicId: string }) => {
  const myImage = cld.image(publicId).format("webp").quality("auto");

  return (
    <AdvancedImage
      cldImg={myImage}
      style={{
        postition: "absolute",
        width: "100%",
        height: "100%",
      }}
    />
  );
};
