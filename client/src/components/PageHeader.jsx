export default function PageHeader({ eyebrow, title, action }) {
  return <header className="page-header"><div><small>{eyebrow}</small><h1>{title}</h1></div>{action}</header>
}

