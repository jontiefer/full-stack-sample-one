import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
//import plugin from '@vitejs/plugin-react';
import react from '@vitejs/plugin-react';

/*** ADD TO ENABLE SSL
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';


// const baseFolder =
//     process.env.APPDATA !== undefined && process.env.APPDATA !== ''
//         ? `${process.env.APPDATA}/ASP.NET/https`
//         : `${process.env.HOME}/.aspnet/https`;

const baseFolder = __dirname

const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : "Developer-UI";

const certificateName = 'developer-ui+3'

if (!certificateName) {
    console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
    process.exit(-1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}-key.pem`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}
***/

// https://vitejs.dev/config/
export default defineConfig({
    base: './',
    build: {
        sourcemap: true
    },
    // build: {
    //     outDir: 'build'
    // },    
    // build: {
    //     rollupOptions: {
    //       external: '/src/main.tsx'
    //     }
    // },
    //plugins: [plugin()],    
    plugins: [
        react()
      ],
    //root: path.join(__dirname, 'src'),
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },    
    server: {        
        host: true,
        port: 8173,
        strictPort: true,
        // Enable to Use SSL
        // https: {
        //     key: fs.readFileSync(keyFilePath),
        //     cert: fs.readFileSync(certFilePath),
        // }
    }
})
