const { initialize } = require('zokrates-js')
const fs = require('fs');

initialize().then((zokratesProvider) => {
    const source =
        `
    import "hashes/mimc7/mimc7";

    def main(private field a) {
        field h = mimc7::<7>(a, 0);
        assert(h == 20770104540633898846502527944698065343915622259733646330486160381217925422992);
        return;
    }

    `;



    const artifacts = zokratesProvider.compile(source);

    while (true) {
        try {
            const { witness } = zokratesProvider.computeWitness(artifacts, [i.toString()]);


            const keypair = zokratesProvider.setup(artifacts.program);
            fs.writeFile('./keypair.key', (JSON.stringify(keypair)), err => {
                if (err) {
                    console.error(err);
                }
            });

            const proof = zokratesProvider.generateProof(artifacts.program, witness, keypair.pk);

            console.log(i);

            var result = JSON.stringify(proof);
            console.log(result);
            return result;
        } catch (err) {
        }

    }
});

