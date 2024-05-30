/** @type {import('next').NextConfig} */
const nextConfig = {
  async redirects() {
    return [
      {
        source: "/api/:slug*",
        destination: `${process.env.API_SERVER_URL}/api/:slug*`,
        permanent: true,
      },
    ];
  }
};

export default nextConfig;
