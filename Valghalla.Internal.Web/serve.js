const { spawn } = require('child_process');
const { writeFileSync, existsSync } = require('fs');
const { join } = require('path');

const appSettingsPath = join(process.cwd(), "../Valghalla.Internal.API/appsettings.development.json");
console.log("Checking appsettings.development.json availability...");

if (!existsSync(appSettingsPath)) {
    console.error("File does not exist. Please check source control documentation on how to create development app settings file");
    process.exit(1);
}

const appSettings = require('../Valghalla.Internal.API/appsettings.development.json');

if (!appSettings.AngularDevServer) {
    console.error("AngularDevServer does not exist in appsettings file. Please check source control documentation on how to create development app settings file");
    process.exit(1);
}

if (!appSettings.Urls) {
    console.error("Urls does not exist in appsettings file. Please check source control documentation on how to create development app settings file");
    process.exit(1);
}

const ngUrl = new URL(appSettings.AngularDevServer);
const dotnetUrl = new URL(appSettings.Urls);
console.log(`Valghalla.Internal.Web Server: ${appSettings.AngularDevServer}`);
console.log(`Valghalla.Internal.API Server: ${appSettings.Urls}`);

const envScript = `/** This file is auto-generated for development purporse. Do not commit to source control. **/
export const environment = {
    baseAddress: "${dotnetUrl.protocol}//${dotnetUrl.hostname}:${dotnetUrl.port}/api/"
}
`;

const envFilePath = join(process.cwd(), "src/environments/environment.development.ts");
writeFileSync(envFilePath, envScript, { encoding: 'utf8' });
console.log("A development file has been created: " + envFilePath);

console.log("Stating Angular dev server...");
const ng = spawn(`ng serve --host ${ngUrl.hostname} --port ${ngUrl.port} --ssl`, [], { shell: true, detached: true });

ng.stdout.on('data', (data) => {
    console.log(`NgServe process stdout: ${data}`);
});
    
ng.stderr.on('data', (data) => {
    console.error(`NgServe process stderr: ${data}`);
});
    
ng.on('close', (code) => {
    console.log(`NgServe process exited with code ${code}`);
});

ng.unref();