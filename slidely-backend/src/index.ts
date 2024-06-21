import express from 'express';
import bodyParser from 'body-parser';
import fs from 'fs';

const app = express();
const port = 3000;

app.use(bodyParser.json());

app.post('/submit', (req, res) => {
    const submission = req.body;
    const db = JSON.parse(fs.readFileSync('db.json', 'utf-8'));
    db.submissions.push(submission);
    fs.writeFileSync('db.json', JSON.stringify(db, null, 2));
    res.status(200).send('Submission received');
});

app.get('/read', (req, res) => {
    const index = parseInt(req.query.index as string, 10);
    const db = JSON.parse(fs.readFileSync('db.json', 'utf-8'));
    if (index >= 0 && index < db.submissions.length) {
        res.status(200).json(db.submissions[index]);
    } else {
        res.status(404).send('Submission not found');
    }
});

app.put('/edit/:index', (req, res) => {
    const index = parseInt(req.params.index, 10);
    const editedSubmission = req.body;
    const db = JSON.parse(fs.readFileSync('db.json', 'utf-8'));
    if (index >= 0 && index < db.submissions.length) {
        db.submissions[index] = editedSubmission;
        fs.writeFileSync('db.json', JSON.stringify(db, null, 2));
        res.status(200).send('Submission edited successfully');
    } else {
        res.status(404).send('Submission not found');
    }
});

app.delete('/delete/:index', (req, res) => {
    const index = parseInt(req.params.index, 10);
    const db = JSON.parse(fs.readFileSync('db.json', 'utf-8'));
    if (index >= 0 && index < db.submissions.length) {
        db.submissions.splice(index, 1);
        fs.writeFileSync('db.json', JSON.stringify(db, null, 2));
        res.status(200).send('Submission deleted successfully');
    } else {
        res.status(404).send('Submission not found');
    }
});

app.get('/search', (req, res) => {
    const email = req.query.email as string;
    const db = JSON.parse(fs.readFileSync('db.json', 'utf-8'));
    
    const submission = db.submissions.find((sub: any) => sub.Email === email);

    if (submission) {
        res.status(200).json(submission);
    } else {
        console.log('Submission not found for email:', email);
    }
});


app.listen(port, () => {
    console.log(`Server is running on http://localhost:${port}`);
});
