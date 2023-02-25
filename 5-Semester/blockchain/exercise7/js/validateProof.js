
const { initialize } = require('zokrates-js')
const fs = require('fs');



initialize().then((zokratesProvider) => {
    if (process.argv.length != 3) {
        console.error("One command line argument required")
        process.exit();
    }
    fs.readFile('./keypair.key', 'utf8', (err, keypair) => {
        if (error) {
            console.error(error);
            process.exit();
        }
        try {
            //verify command proof
            const isVerified = zokratesProvider.verify(JSON.parse(keypair).vk, JSON.parse(process.argv[2]));
            console.log(isVerified);
            return isVerified;
        } catch (e) {
            console.error("Invalid Json.")
            return false;
        }

    });
});

