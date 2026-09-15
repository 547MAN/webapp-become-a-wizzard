import { Badge, Button, Card } from 'react-bootstrap'

export default function QuizCard({ quiz, onPlay, onEdit, onDelete }) {
  return <Card className="quiz-card h-100"><Card.Body>
    <div className="d-flex justify-content-between"><Badge bg={quiz.isPublished ? 'success' : 'secondary'}>{quiz.isPublished ? 'Publisert' : 'Kladd'}</Badge>{quiz.bossFightEnabled && <span className="boss-label">Boss</span>}</div>
    <Card.Title>{quiz.title}</Card.Title><Card.Text>{quiz.description}</Card.Text>
    <div className="quiz-meta"><span>{quiz.topic}</span><span>{quiz.questionCount} spørsmål</span></div>
    <div className="d-flex gap-2 mt-3">{onPlay && <Button onClick={onPlay}>Spill</Button>}{onEdit && <Button variant="outline-light" onClick={onEdit}>Rediger</Button>}{onDelete && <Button variant="outline-danger" onClick={onDelete}>Slett</Button>}</div>
  </Card.Body></Card>
}

