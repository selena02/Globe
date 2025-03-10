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
    .resize(thumbnail().width(100).height(100))
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

export const bigProfileImg = ({ publicId }: { publicId: string }) => {
  const myImage = cld
    .image(publicId)
    .resize(thumbnail().width(200).height(200))
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
  const myImage = cld
    .image(publicId)
    .resize(thumbnail().width(400))
    .format("webp")
    .quality("auto");

  return (
    <AdvancedImage
      cldImg={myImage}
      style={{
        position: "absolute",
        top: 0,
        left: 0,
        width: "100%",
        height: "100%",
        objectFit: "cover",
        objectPosition: "center",
      }}
    />
  );
};

export const FullPostImg = ({ publicId }: { publicId: string }) => {
  const myImage = cld
    .image(publicId)
    .resize(thumbnail().width(1000))
    .format("webp")
    .quality("auto");

  return (
    <AdvancedImage
      cldImg={myImage}
      style={{
        width: "100%",
        height: "100%",
        objectFit: "cover",
        objectPosition: "center",
      }}
    />
  );
};

export const LandmarkImg = ({ publicId }: { publicId: string }) => {
  const myImage = cld
    .image(publicId)
    .resize(thumbnail().width(600))
    .format("webp")
    .quality("auto");

  return (
    <div className="full-post-img">
      <AdvancedImage
        cldImg={myImage}
        style={{
          position: "absolute",
          top: 0,
          left: 0,
          width: "100%",
          height: "100%",
          objectFit: "cover",
          objectPosition: "center",
        }}
      />
    </div>
  );
};
