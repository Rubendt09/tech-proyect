import type { Post } from ".";

export const statuses: Post["status"][] = [
  "to_do",
  "in_process",
  "done"
];

export const statusNames: Record<Post["status"], string> = {
  to_do: "To do",
  in_process: "In Process",
  done: "Done",
};

export type PostsByStatus = Record<Post["status"], Post[]>;

export const getPostsByStatus = (unorderedPosts: Post[]) => {
  const postsByStatus: PostsByStatus = unorderedPosts.reduce(
    (acc, post) => {
      acc[post.status].push(post);
      return acc;
    },
    statuses.reduce(
      (obj, status) => ({ ...obj, [status]: [] }),
      {} as PostsByStatus
    )
  );
  
  statuses.forEach((status) => {
    postsByStatus[status] = postsByStatus[status].sort(
      (recordA: Post, recordB: Post) => recordA.index - recordB.index
    );
  });
  return postsByStatus;
};